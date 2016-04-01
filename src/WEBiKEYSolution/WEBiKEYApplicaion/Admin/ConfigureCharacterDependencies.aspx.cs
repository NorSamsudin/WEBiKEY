using System;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WEBiKEY.Application.Admin
{
    public partial class ConfigureCharacterDependencies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateComboBoxes();
                this.DataBind();
            }
        }

        private void PopulateComboBoxes()
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                cmbCharacter.Items.Add(new ListItem("--Select--", "-1"));
                cmbCharacter.Items.AddRange(context.Characters.OrderBy(i => i.CharacterCode).Select(i => new ListItem() { Text = i.CharacterCode, Value = SqlFunctions.StringConvert((decimal)i.CharacterID) }).ToArray());
                cmbCharacter_SelectedIndexChanged(null, null);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int stateId = int.Parse(cmbState.SelectedValue);
            int disCharId = int.Parse(cmbDisabledCharacter.SelectedValue);

            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var state = context.CharacterStates.Single(i => i.CharacterStateID == stateId);
                var character = context.Characters.Find(disCharId);

                if (state == null || character == null)
                {
                    throw new Exception("Invalid Character State or Disabling character is selected!");
                }

                state.DisabledCharacters.Add(character);
                context.SaveChanges();

                gvExistingDependencies.DataBind();
            }
        }

        protected void cmbDisabledCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int disCharId = int.Parse(cmbDisabledCharacter.SelectedValue);
            if (disCharId == -1)
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = true;
            }
        }

        protected void cmbCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                int charId = int.Parse(cmbCharacter.SelectedValue);
                cmbState.Items.Clear();

                if (charId != -1)
                {
                    cmbState.Items.Add(new ListItem("--Select--", "-1"));
                    cmbState.Items.AddRange(context.CharacterStates.Where(i => i.CharacterID == charId).Select(i => new ListItem() { Text = i.CharacterStateCode, Value = SqlFunctions.StringConvert((decimal)i.CharacterStateID) }).ToArray());
                }
                else
                {
                    cmbState.Items.Add(new ListItem("--Select Character--", "-1"));
                    cmbState_SelectedIndexChanged(sender, e);
                }

                cmbState_SelectedIndexChanged(null, null);
            }
        }

        protected void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            int stateId = int.Parse(cmbState.SelectedValue);
            cmbDisabledCharacter.Items.Clear();

            if (stateId != -1)
            {
                using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                {
                    cmbDisabledCharacter.Items.Add(new ListItem("--Select--", "-1"));
                    cmbDisabledCharacter.Items.AddRange(context.Characters.OrderBy(i => i.CharacterCode).Select(i => new ListItem() { Text = i.CharacterCode, Value = SqlFunctions.StringConvert((decimal)i.CharacterID) }).ToArray());
                }
            }
            else
            {
                cmbDisabledCharacter.Items.Add(new ListItem("--Select State--", "-1"));
            }

            cmbDisabledCharacter_SelectedIndexChanged(sender, e);
        }

        protected void gvExistingDependencies_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CustomDelete")
            {
                // get the stateId of the clicked row
                string args = e.CommandArgument.ToString().Trim(new char[] { ' ', '(', ')' });
                string[] tuple = args.Split(new char[] { ',' });

                int characterStateID = Convert.ToInt32(tuple[0]);
                int disabledCharacterID = Convert.ToInt32(tuple[1]);

                // Delete the record 
                RemoveDependencyForCharacterState(characterStateID, disabledCharacterID);
            }
        }

        private void RemoveDependencyForCharacterState(int characterStateID, int disabledCharacterID)
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var state = context.CharacterStates.Find(characterStateID);
                var disabledCharacter = context.Characters.Find(disabledCharacterID);

                state.DisabledCharacters.Remove(disabledCharacter);
                context.SaveChanges();

                gvExistingDependencies.DataBind();
            }

        }

        protected void gvExistingDependencies_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
                l.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure you want to delete the dependency for: " +
                DataBinder.Eval(e.Row.DataItem, "CharacterCode") + "')");
            }
        }
    }
}