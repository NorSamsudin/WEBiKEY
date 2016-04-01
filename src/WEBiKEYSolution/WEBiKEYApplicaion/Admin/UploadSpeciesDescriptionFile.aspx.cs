using System;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using WEBiKEY.Application.Classes;

namespace WEBiKEY.Application.Admin
{
    public partial class UploadSpeciesDescriptionFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateComboBoxes();
            }
        }

        private void PopulateComboBoxes()
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                cmbSpecies.Items.Add(new ListItem("--Select--", "-1"));
                cmbSpecies.Items.AddRange(context.Species.OrderBy(i => i.SpeciesName).Select(i => new ListItem() { Text = i.SpeciesName, Value = SqlFunctions.StringConvert((decimal)i.SpeciesID) }).ToArray());
                cmbSpecies_SelectedIndexChanged(null, null);
            }
        }

        protected void cmbSpecies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int speciesId = int.Parse(cmbSpecies.SelectedValue);
            if (speciesId != -1)
            {
                btnUpload.Enabled = true;
                FileUploadDescriptionFile.Enabled = true;
                btnRemoveCurrentFile.Enabled = true;
                lblInfo.Text = string.Empty;
            }
            else
            {
                btnUpload.Enabled = false;
                FileUploadDescriptionFile.Enabled = false;
                btnRemoveCurrentFile.Enabled = false;
            }
        }

       
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";

            if (FileUploadDescriptionFile.HasFile)
            {
                if (FileUploadDescriptionFile.PostedFile.ContentLength > 5000000)
                {
                    lblInfo.Text = "File size must be less than 5Mb.";
                    return;
                }

                string[] acceptedFileTypes = new string[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
                string Extension = Path.GetExtension(FileUploadDescriptionFile.PostedFile.FileName);
                string fileName = Path.GetFileName(FileUploadDescriptionFile.PostedFile.FileName);

                if (!acceptedFileTypes.Contains(Extension.ToLower()))
                {
                    lblInfo.Text = "Please select .PDF, .DOCX, .DOC, .XLSX or XLS file.";
                }
                else
                {
                    MemoryStream ms = new MemoryStream(FileUploadDescriptionFile.FileBytes);
                    int spId = int.Parse(cmbSpecies.SelectedValue);

                    using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                    {
                        var sp = context.Species.Single(i => i.SpeciesID == spId);
                        sp.DescriptionFile = ms.ToArray();
                        sp.DescriptionFileName = fileName;
                        context.SaveChanges();

                        lblInfo.Text = "File uploaded successfully.";
                    }
                }
            }
            else
            {
                lblInfo.Text = "Please Select a file first.";
            }
        }

        protected void btnRemoveCurrentFile_Click(object sender, EventArgs e)
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                int spId = int.Parse(cmbSpecies.SelectedValue);
                var sp = context.Species.Single(i => i.SpeciesID == spId);
                sp.DescriptionFile = null;
                sp.DescriptionFileName = null;
                context.SaveChanges();

                lblInfo.Text = "File cleared.";
            }
        }

        protected string GetImageUrl(object image)
        {
            return Utils.GetImageUrl((byte[])image);
        }

        //protected void btnUploadImage_Click(object sender, EventArgs e)
        //{
        //    if (FileUploadImage.HasFile)
        //    {
        //        lblImageInfo.Text = "File uploaded successfully.";
        //    }
        //    else
        //    {
        //        lblImageInfo.Text = "Please Select a file first.";
        //    }
        //}
    }
}