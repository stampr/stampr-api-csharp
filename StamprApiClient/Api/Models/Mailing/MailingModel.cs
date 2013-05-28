using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Security.Cryptography;

namespace StamprApiClient.Api.Models.Mailing
{
    public class MailingModel
    {
        public int Mailing_Id { get; internal set; }

        public int User_Id { get; internal set; }

        public string Printer_Id { get; internal set; }

        public string Pdf { get; internal set; }

        public Status Status { get; internal set; }

        public DateTime Initiated { get; internal set; }

        public int Batch_Id { get; set; }

        public string Address { get; set; }

        public string ReturnAddress { get; set; }

        public Format Format { get; set; }

        public IDictionary<string, string> Data { get; set; }

        internal IDictionary<string, object> ToPostPropertiesDictionary()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("batch_id", Batch_Id);
            properties.Add("address", Address);
            properties.Add("returnaddress", ReturnAddress);
            properties.Add("format", Format);
            if (Data != null)
            {
                string data = new JavaScriptSerializer().Serialize(Data);
                string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
                using(MD5 md5 = new MD5CryptoServiceProvider())
                {
                    byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(base64Data));
                    string result = BitConverter.ToString(checkSum).Replace("-", string.Empty).ToLower();
                    properties.Add("data", base64Data);
                    properties.Add("md5", result);
                }
            }

            return properties;
        }
    }
}
