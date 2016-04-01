using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace WEBiKEY.Application
{
    public partial class Key : System.Web.UI.Page
    {
        private const string CAT_LIST_KEY = "ShowCategoriesList";
        private const string SELECTED_CHAR_STATES_KEY = "States";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[CAT_LIST_KEY] == null)
            {
                Response.Redirect("~/SelectCategories.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    imgCharacterImage.Visible = false;

                    using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                    {
                        List<int> selectedCatIds = Session[CAT_LIST_KEY] as List<int>;

                        var chars = context.CharacterCategories.Include(i => i.Characters).Include(i => i.Characters.Select(j => j.CharacterStates)).Where(i => selectedCatIds.Contains(i.CategoryID)).ToList();
                        DataListCategories.DataSource = chars;

                        DataListCategories.DataBind();

                        UpdateSpeciesListForRadList();
                        EnableDisableCharacters();
                    }
                }
            }
        }

        protected void btnViewImage_Click(object sender, EventArgs e)
        {
            string charCode = ((((sender as Button).Parent) as DataListItem).FindControl("lblCharacterCode") as Label).Text;
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var character = context.Characters.Single(i => i.CharacterCode == charCode);
                if (character.Image != null)
                {
                    imgCharacterImage.Visible = true;
                    imgCharacterImage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(character.Image);
                    h4Images.InnerText = string.Format("Character: {0}", character.CharacterDescription);
                    h4ImageCopyrights.Visible = true;
                }
                else
                {
                    imgCharacterImage.Visible = false;
                    imgCharacterImage.ImageUrl = "#";
                    h4Images.InnerText = string.Format("{0}: No Image.", charCode);
                    h4ImageCopyrights.Visible = false;
                }
            }
        }

        private void UpdateSpeciesListForRadList()
        {
            HashSet<int> selectedStateIds = new HashSet<int>();

            foreach (DataListItem catItem in DataListCategories.Items)
            {
                DataList dlChars = catItem.FindControl("DataListCharacters") as DataList;
                foreach (DataListItem charItem in dlChars.Items)
                {
                    RadioButtonList radList = charItem.FindControl("RadioButtonListCharacterStates") as RadioButtonList;

                    if (radList.SelectedItem != null)
                    {
                        int selectedVal = int.Parse(radList.SelectedValue);
                        selectedStateIds.Add(selectedVal);
                    }
                }
            }

            ViewState[SELECTED_CHAR_STATES_KEY] = selectedStateIds;

            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var l = new List<int>(new int[] { 1, 2 });
                var cs = context.CharacterStates.Where(s => selectedStateIds.Contains(s.CharacterStateID));
                IQueryable<Species> sp = context.Species.Where(i => false);
                if (cs.Count() > 0)
                {
                    sp = context.Species.Where(s => (cs.All(c => s.CharacterStates.Contains(c))));
                }

                //var data = context.Species.Where(i => selectedStates.Values.All(ss => i.CharacterStates.Select(s => s.CharacterStateID).Contains(ss)));

                Species[] ds = null;
                if (sp != null && sp.Count() > 0)
                {
                    ds = sp.ToArray();
                }
                ListBoxSpeciesIncl.DataSource = null;
                ListBoxSpeciesIncl.Items.Clear();
                ListBoxSpeciesIncl.DataSource = ds;

                ListBoxSpeciesIncl.DataTextField = "SpeciesName";
                ListBoxSpeciesIncl.DataValueField = "SpeciesID";

                var data2 = context.Species.Except(sp);
                //var data2 = context.Species.Where(i => !selectedStates.Values.All(j => i.CharacterStates.Select(s => s.CharacterStateID).Contains(j)));
                Species[] ds2 = null;
                if (data2.Count() > 0)
                {
                    ds2 = data2.ToArray();
                }
                ListBoxSpeciesExcl.DataSource = null;
                ListBoxSpeciesExcl.Items.Clear();
                ListBoxSpeciesExcl.DataSource = ds2;
                ListBoxSpeciesExcl.DataTextField = "SpeciesName";
                ListBoxSpeciesExcl.DataValueField = "SpeciesID";
            }

            ListBoxSpeciesIncl.DataBind();
            ListBoxSpeciesExcl.DataBind();
        }

        private HashSet<int> UpdateSelectedStates(RadioButtonList radList)
        {
            HashSet<int> selectedStateIds = new HashSet<int>();

            if (radList == null)
            {
                return selectedStateIds;
            }

            if (ViewState[SELECTED_CHAR_STATES_KEY] == null)
            {
                ViewState[SELECTED_CHAR_STATES_KEY] = selectedStateIds;
            }

            selectedStateIds = ViewState[SELECTED_CHAR_STATES_KEY] as HashSet<int>;

            if (string.IsNullOrEmpty(radList.SelectedValue))
            {
                foreach (ListItem item in radList.Items)
                {
                    selectedStateIds.Remove(int.Parse(item.Value));
                }
            }
            else
            {
                int selectedStateId = int.Parse(radList.SelectedValue);
                selectedStateIds.Add(selectedStateId);
            }

            ViewState[SELECTED_CHAR_STATES_KEY] = selectedStateIds;
            return selectedStateIds;
        }

        protected void EnableDisableCharacters()
        {
            HashSet<int> selectedStateIds = ViewState[SELECTED_CHAR_STATES_KEY] as HashSet<int>;

            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var disabledCharacterIds = context.CharacterStates.Where(i => selectedStateIds.Contains(i.CharacterStateID)).SelectMany(i => i.DisabledCharacters).Select(i => i.CharacterID).ToList();
                //context.Characters.Where(i=>i.DisablingCharacterStates.Count>0) CharacterStates.AsNoTracking().Where(i => i.DisabledCharacterID != null && selectedStateIds.Contains(i.CharacterStateID)).Select(i => i.DisabledCharacterID.Value).Distinct().ToList();

                foreach (DataListItem catItem in DataListCategories.Items) //each category
                {
                    DataList dlChars = catItem.FindControl("DataListCharacters") as DataList;
                    foreach (DataListItem charItem in dlChars.Items) //each character
                    {
                        string chaIdStr = (charItem.FindControl("lblCharacterId") as Label).Text;

                        if (string.IsNullOrEmpty(chaIdStr))
                        {
                            return;
                        }

                        int chaId = int.Parse(chaIdStr);

                        if (disabledCharacterIds.Contains(chaId)) //there is a disable character
                        {
                            charItem.Enabled = false;

                            RadioButtonList radList = charItem.FindControl("RadioButtonListCharacterStates") as RadioButtonList;
                            if (radList != null)
                            {
                                foreach (ListItem rad in radList.Items)
                                {
                                    rad.Selected = false;
                                }

                                //radList.SelectedIndex = -1;
                            }
                        }
                        else //no selected states, keep disabled
                        {
                            charItem.Enabled = true;
                        }
                    }
                }
            }
        }

        protected void btnResetCharacter_Click(object sender, EventArgs e)
        {
            RadioButtonList radList = ((sender as Button).Parent as DataListItem).FindControl("RadioButtonListCharacterStates") as RadioButtonList;
            foreach (ListItem rad in radList.Items)
            {
                rad.Selected = false;
            }
            //radList.SelectedIndex = -1;

            RadioButtonListCharacterStates_SelectedIndexChanged(sender, e);
            RadioButtonListCharacterStates_SelectedIndexChanged(sender, e);

        }

        protected void RadioButtonListCharacterStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpeciesListForRadList();
            EnableDisableCharacters();
            UpdateSpeciesListForRadList();
            EnableDisableCharacters();
        }

        protected void btnResetAllStates_Click(object sender, EventArgs e)
        {
            foreach (DataListItem catItem in DataListCategories.Items)
            {
                DataList dlChars = catItem.FindControl("DataListCharacters") as DataList;
                foreach (DataListItem charItem in dlChars.Items)
                {
                    RadioButtonList radList = charItem.FindControl("RadioButtonListCharacterStates") as RadioButtonList;
                    radList.SelectedIndex = -1;
                }
            }

            ViewState[SELECTED_CHAR_STATES_KEY] = null;
            RadioButtonListCharacterStates_SelectedIndexChanged(null, null);
        }

        protected void btnHideImage_Click(object sender, EventArgs e)
        {
            imgCharacterImage.Visible = false;
            imgCharacterImage.ImageUrl = "#";
            h4Images.InnerText = string.Empty;
            h4ImageCopyrights.Visible = false;
        }
    }
}