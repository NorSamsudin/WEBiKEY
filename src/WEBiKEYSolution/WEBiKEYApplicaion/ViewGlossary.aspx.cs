using System;
using System.Linq;
using WEBiKEY.Application.Classes;

namespace WEBiKEY.Application
{
    public partial class ViewGlossary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                var glossaryFile = context.UploadedFiles.SingleOrDefault(i => i.FileType == UploadFileTypes.GlossaryFileType);
                if (glossaryFile != null)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + glossaryFile.FileName);
                    Response.AddHeader("Content-Length", glossaryFile.FileData.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(glossaryFile.FileData);
                    Response.End();
                }
            }
        }
    }
}