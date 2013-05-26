using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace StamprApiClient.Api.Models.Config
{
    public class ConfigModel
    {
        public int Config_Id { get; internal set; }
        public int User_Id { get; internal set; }
        public bool ReturnEnvelope { get; internal set; }
        public string Version { get; internal set; }
        public Size Size { get; internal set; }
        public Output Output { get; internal set; }
        public Turnaround Turnaround { get; internal set; }
        public Style Style { get; internal set; }

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
