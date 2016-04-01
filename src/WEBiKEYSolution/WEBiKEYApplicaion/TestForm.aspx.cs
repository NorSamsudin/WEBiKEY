using System;
using System.Web.UI.WebControls;

namespace WEBiKEY.Application
{
    public partial class TestForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (ListItem item in RadioButtonList1.Items)
            {
                item.Attributes.Add("class", "radio-style");
            }

            if (!IsPostBack)
            {

                DataListCategories.DataSource = new test[] { 
                    new test { Id = 1, Val = "aa", Sub = new string[] { "11", "12" } },
                    new test { Id = 2, Val = "bb", Sub = new string[] { "21", "22", "23" } },
                };

                DataListCategories.DataBind();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }

    public class test
    {
        public int Id { get; set; }
        public string Val { get; set; }
        public string[] Sub { get; set; }
    }
}