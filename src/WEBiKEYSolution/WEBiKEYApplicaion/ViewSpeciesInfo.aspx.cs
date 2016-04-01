using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using WEBiKEY.Application.Classes;

namespace WEBiKEY.Application
{
    public partial class ViewSpeciesInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetDownloadButtonText(object value)
        {
            if (value == null)
            {
                return "No File";
            }

            return "Download";
        }

        public bool IsDescriptionFileAvailable(object value)
        {
            if (value == null)
            {
                return false;
            }
            return true;
        }

        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int speciesID = int.Parse(btn.ValidationGroup);

            using (InteractiveKeyEntities context = new InteractiveKeyEntities())
            {
                byte[] fileData = context.Species.Single(i => i.SpeciesID == speciesID).DescriptionFile;
                string fileName = context.Species.Single(i => i.SpeciesID == speciesID).DescriptionFileName;
                string fileExtension = Path.GetExtension(fileName);

                Response.Clear();
                MemoryStream ms = new MemoryStream(fileData);
                Response.ContentType = Utils.GetMIMETypeForExtension(fileExtension);
                var cdFileHeader = new System.Net.Mime.ContentDisposition() { FileName = fileName };
                Response.AddHeader("content-disposition", cdFileHeader.ToString());
                //Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", Server.HtmlEncode(fileName)));
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
        }

        protected string GetImageUrl(object image)
        {
            return Utils.GetImageUrl((byte[])image);
        }


    }
}