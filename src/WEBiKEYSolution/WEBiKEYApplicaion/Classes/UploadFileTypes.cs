using System.Collections.Generic;
using System.Linq;

namespace WEBiKEY.Application.Classes
{
    public class UploadFileTypes
    {
        public static string GlossaryFileType = "Glossary";

        public static List<string> GetAllFileTypes()
        {
            return (typeof(UploadFileTypes)).GetFields().Select(i => i.GetValue(i).ToString()).ToList();
        }
    }
}