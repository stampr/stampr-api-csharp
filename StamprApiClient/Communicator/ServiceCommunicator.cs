using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using StamprApiClient.Communicator.Models;
using StamprApiClient.Communicator.Abstract;

namespace StamprApiClient.Communicator
{
    internal class ServiceCommunicator : IServiceCommunicator
    {

        public ServiceResponse GetRequest(string url, string authorization)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = authorization;
            return Send(request);
        }

        public ServiceResponse DeleteRequest(string url, string authorization)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Headers["Authorization"] = authorization;
            return Send(request);
        }

        public ServiceResponse PostRequest(string url, string authorization, IDictionary<string, object> formPropertiesValues)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["Authorization"] = authorization;
            if (formPropertiesValues != null && formPropertiesValues.Count > 0)
            {
                IEnumerable<string> properties = formPropertiesValues.Select(propertyValue => string.Format("{0}={1}", propertyValue.Key, propertyValue.Value == null ? null : propertyValue.Value.ToString()));
                string data = string.Join("&", properties);
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
            }

            return Send(request);
        }

        private ServiceResponse Send(HttpWebRequest request)
        {
            var responseFromServer = new ServiceResponse();
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    responseFromServer = ServiceResponse.CreateServiceResponse((HttpWebResponse)response);
                }
            }
            catch (WebException webException)
            {
                HttpWebResponse response = webException.Response as HttpWebResponse;
                responseFromServer = ServiceResponse.CreateServiceResponse(response);
            }

            return responseFromServer;
        }
    }
}
