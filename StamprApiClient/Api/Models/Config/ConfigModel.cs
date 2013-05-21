using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace StamprApiClient.Api.Models.Config
{
    public class ConfigModel
    {
        public int Config_Id { get; set; }
        public int User_Id { get; set; }
        public bool ReturnEnvelope { get; set; }
        public string Version { get; set; } 
        public Size Size { get; set; }
        public Output Output { get; set; }
        public Turnaround Turnaround { get; set; }
        public Style Style { get; set; }

        internal IDictionary<string, object> ToPostPropertiesDictionary()
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("returnenvelope", ReturnEnvelope.ToString().ToLower());
            properties.Add("size", Size);
            properties.Add("output", Output);
            properties.Add("turnaround", Turnaround);
            properties.Add("style", Style);
            return properties;
        }
    }
}
