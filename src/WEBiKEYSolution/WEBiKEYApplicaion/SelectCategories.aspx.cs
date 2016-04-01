using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WEBiKEY.Application
{
    public partial class SelectCategories : System.Web.UI.Page
    {
        private const string CAT_LIST_KEY = "ShowCategoriesList";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCheckboxList();
            }
        }

        private void PopulateCheckboxList()
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var items = context.CharacterCategories.ToList();
                //chkLstCategories.Items.AddRange(context.CharacterCategories.OrderBy(i => i.CategoryName).Select(i => new ListItem { Text = i.CategoryName, Value = System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)i.CategoryID) }).ToArray());
                chkLstCategories.DataSource = items;
                chkLstCategories.DataTextField = "CategoryName";
                chkLstCategories.DataValueField = "CategoryID";

                chkLstCategories.DataBind();

                if (Session[CAT_LIST_KEY] != null)
                {
                    List<int> selectedCatIds = Session[CAT_LIST_KEY] as List<int>;
                    foreach (int id in selectedCatIds)
                    {
                        chkLstCategories.Items.FindByValue(id.ToString()).Selected = true;
                    }
                }

            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            List<int> selectedCatIds = new List<int>();
            bool foundSelects = false;
            lblInfo.Text = "";

            foreach (ListItem chkItem in chkLstCategories.Items)
            {
                if (chkItem.Selected)
                {
                    foundSelects = true;
                    selectedCatIds.Add(int.Parse(chkItem.Value));
                }
            }

            if (foundSelects)
            {
                Session[CAT_LIST_KEY] = selectedCatIds;
                Response.Redirect("~/Key.aspx");
            }
            else
            {
                lblInfo.Text = "Please select at least one Category.";
            }
        }

        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in chkLstCategories.Items)
            {
                item.Selected = true;
            }
        }

        protected void btnSelectNone_Click(object sender, EventArgs e)
        {
            chkLstCategories.ClearSelection();

            //foreach (ListItem item in chkLstCategories.Items)
            //{
            //    item.Selected = false;
            //}
        }
    }
}