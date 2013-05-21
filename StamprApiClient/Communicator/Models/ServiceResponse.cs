using System.IO;
using System.Net;

namespace StamprApiClient.Communicator.Models
{
    internal class ServiceResponse
    {
        public string Response { get; set; }
        public string Status { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceResponse CreateServiceResponse(HttpWebResponse response)
        {
            ServiceResponse responseFromServer = new ServiceResponse();
            if (response != null)
            {
                responseFromServer.Status = response.StatusDescription;
                responseFromServer.StatusCode = response.StatusCode;
                // Reads response
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer.Response = reader.ReadToEnd();
                    }
                }
            }

            return responseFromServer;
        }
    }
}
