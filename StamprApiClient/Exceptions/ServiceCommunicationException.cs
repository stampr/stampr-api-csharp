using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace StamprApiClient.Exceptions
{
    public class ServiceCommunicationException : ApplicationException
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
