using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;

namespace StamprApiClient.Convertor
{
    internal class ModelConvertor
    {
        private readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();

        internal T ConvertToModel<T>(string response)
        {
            object obj = JavaScriptSerializer.DeserializeObject(response);
            return (T)ConvertObject(obj,typeof(T));
        }

        protected JavaScriptSerializer JavaScriptSerializer
        {
            get
            {
                return _javaScriptSerializer;
            }
        }

        protected virtual object GetPropertyValue(string name, Type type, IDictionary<string, object> dictionary)
        {
            return ConvertObject(dictionary[name], type);
        }

        private object ConvertObject(object obj, Type type)
        {
            if (type.IsArray)
            {
                Array array = obj as Array;
                if (array == null)
                {
                    array = Array.CreateInstance(typeof(object), 1);
                    array.SetValue(obj, 0);
                }

                Array resultArray = Array.CreateInstance(type.GetElementType(), array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    resultArray.SetValue(ConvertObject(array.GetValue(i), type.GetElementType()), i);
                }

                return resultArray;
            }
            else
            {
                IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
                if (dictionary == null)
                {
                    return JavaScriptSerializer.ConvertToType(obj, type);
                }

                object objectToReturn = Activator.CreateInstance(type);
                PropertyInfo[] pinfos = type.GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);
                IDictionary<string, object> caseInsensitiveDictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
                foreach(PropertyInfo pinfo in pinfos)
                {
                    if (pinfo.CanWrite && caseInsensitiveDictionary.ContainsKey(pinfo.Name))
                    {
                        object value = GetPropertyValue(pinfo.Name, pinfo.PropertyType, caseInsensitiveDictionary);
                        pinfo.SetValue(objectToReturn, value, null);
                    }
                }

                return objectToReturn;
            }
        }
    }
}
