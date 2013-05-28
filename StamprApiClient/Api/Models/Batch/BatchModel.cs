using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using StamprApiClient.Api.Models.Search;

namespace StamprApiClient.Api.Models.Batch
{
    public class BatchModel
    {
        public int Batch_Id { get; set; }
        public int User_Id { get; internal set; }
        public int Config_Id { get; set; }
        public Status Status { get; set; }
        public string Template { get; set; }
        public string Version { get; internal set; } 

        internal IDictionary<string, object> ToPostPropertiesDictionary()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("config_id", Config_Id);
            properties.Add("status", Status);
            properties.Add("template", Template);
            return properties;
        }

        internal IDictionary<string, object> ToUpdatePropertiesDictionary()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("status", Status);
            return properties;
        }
    }
}
