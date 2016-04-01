using System;
using System.IO;
using System.Linq;
using WEBiKEY.Application.Classes;

namespace WEBiKEY.Application.Admin
{
    public partial class UploadGlossaryFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";

            if (FileUploadDescriptionFile.HasFile)
            {
                //if (FileUploadDescriptionFile.PostedFile.ContentLength > 1048576)
                //{
                //    lblInfo.Text = "File size must be less than 1Mb.";
                //    return;
                //}

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

                    using (InteractiveKeyEntities context = new InteractiveKeyEntities())
                    {
                        var file = context.UploadedFiles.SingleOrDefault(i => i.FileType == UploadFileTypes.GlossaryFileType);
                        if (file == null)
                        {
                            file = context.UploadedFiles.Create();
                            file.FileType = UploadFileTypes.GlossaryFileType;
                            context.UploadedFiles.Add(file);
                        }

                        file.FileName = fileName;
                        file.FileData = ms.ToArray();
                        ms.Dispose();

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
    }
}