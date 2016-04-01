using System;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace WEBiKEY.Application.Admin
{
    public partial class UploadCharacterImages : System.Web.UI.Page
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
                cmbCharacters.Items.Add(new ListItem("--Select--", "-1"));
                cmbCharacters.Items.AddRange(context.Characters.OrderBy(i => i.CharacterCode).Select(i => new ListItem() { Text = i.CharacterCode, Value = SqlFunctions.StringConvert((decimal)i.CharacterID) }).ToArray());
                cmbCharacters_SelectedIndexChanged(null, null);
            }
        }

        protected void cmbCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            int charId = int.Parse(cmbCharacters.SelectedValue);
            if (charId != -1)
            {
                btnUpload.Enabled = true;
                FileUploadCharacterImage.Enabled = true;

                using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                {
                    var character = context.Characters.Single(i => i.CharacterID == charId);
                    if (character.Image != null)
                    {
                        CharacterImage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(character.Image);
                        CharacterImage.Visible = true;
                        btnRemoveImage.Visible = true;
                    }
                    else
                    {
                        CharacterImage.ImageUrl = "#";
                        CharacterImage.Visible = false;
                        btnRemoveImage.Visible = false;
                    }
                }
            }
            else
            {
                btnUpload.Enabled = false;
                FileUploadCharacterImage.Enabled = false;
                CharacterImage.ImageUrl = "#";
                CharacterImage.Visible = false;
                btnRemoveImage.Visible = false;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";

            if (FileUploadCharacterImage.HasFile)
            {
                if (FileUploadCharacterImage.PostedFile.ContentLength > 2097152)
                {
                    lblInfo.Text = "File size must be less than 2 Mb.";
                    return;
                }

                string[] acceptedImageTypes = new string[] { ".jpg", ".png", ".gif" };
                string Extension = Path.GetExtension(FileUploadCharacterImage.PostedFile.FileName);
                if (!acceptedImageTypes.Contains(Extension.ToLower()))
                {
                    lblInfo.Text = "Please select .JPG, .PNG or .GIF file.";
                }
                else
                {
                    MemoryStream ms = new MemoryStream(FileUploadCharacterImage.FileBytes);
                    int charId = int.Parse(cmbCharacters.SelectedValue);

                    using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                    {
                        var cha = context.Characters.Single(i => i.CharacterID == charId);
                        cha.Image = ms.ToArray();
                        context.SaveChanges();
                        cmbCharacters_SelectedIndexChanged(sender, e);
                        lblInfo.Text = "Image uploaded successfully";
                    }
                }
            }
            else
            {
                lblInfo.Text = "Please Select a file first.";
            }
        }

        protected void btnRemoveImage_Click(object sender, EventArgs e)
        {
            int charId = int.Parse(cmbCharacters.SelectedValue);
            if (charId != -1)
            {
                using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                {
                    var character = context.Characters.Single(i => i.CharacterID == charId);
                    character.Image = null;
                    context.SaveChanges();
                    lblInfo.Text = "Successfully removed the image";
                    cmbCharacters_SelectedIndexChanged(sender, e);
                }
            }
        }
    }
}