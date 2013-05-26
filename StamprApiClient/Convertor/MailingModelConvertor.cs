using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StamprApiClient.Convertor
{
    internal class MailingModelConvertor : ModelConvertor
    {
        protected override object GetPropertyValue(string name, Type type, IDictionary<string, object> dictionary)
        {
            if (type != typeof(IDictionary<string, string>))
            {
                return base.GetPropertyValue(name, type, dictionary);
            }

            string value = base.GetPropertyValue(name, typeof(string), dictionary) as string;
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            
            return JavaScriptSerializer.Deserialize<IDictionary<string, string>>(value);
        }
    }
}
