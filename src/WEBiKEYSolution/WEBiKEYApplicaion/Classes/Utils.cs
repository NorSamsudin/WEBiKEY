using System;

namespace WEBiKEY.Application.Classes
{
    public class Utils
    {
        public static string GetMIMETypeForExtension(string fileExtension)
        {

            string extWithoutDot = fileExtension.Trim().TrimStart(new char[] { '.' });

            switch (extWithoutDot)
            {
                case "pdf":
                    return "application/pdf";
                case "doc":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "xls":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default:
                    return "application/octet-stream";
            }
        }

        public static string GetImageUrl(byte[] image)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String(image);
        }
    }
}