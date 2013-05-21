using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StamprApiClient.Api.Models.Mailing
{
    public class MailingModel
    {
        public int Mailing_Id { get; set; }

        public int User_Id { get; set; }

        public string Printer_Id { get; set; }

        public string Pdf { get; set; }

        public Status Status { get; set; }

        public DateTime Initiated { get; set; }

        public int Batch_Id { get; set; }

        public string Address { get; set; }

        public string ReturnAddress { get; set; }

        public Format Format { get; set; }

        public string Data { get; set; }

        public string Md5 { get; set; }

        internal IDictionary<string, object> ToPostPropertiesDictionary()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("batch_id", Batch_Id);
            properties.Add("address", Address);
            properties.Add("returnaddress", ReturnAddress);
            properties.Add("format", Format);
            if (!string.IsNullOrEmpty(Data))
            {
                properties.Add("data", Data);
            }

            if (!string.IsNullOrEmpty(Md5))
            {
                properties.Add("md5", Md5);
            }

            return properties;
        }
    }
}
