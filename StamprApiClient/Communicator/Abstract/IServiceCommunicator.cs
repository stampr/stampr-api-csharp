using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Communicator.Models;

namespace StamprApiClient.Communicator.Abstract
{
    internal interface IServiceCommunicator
    {
        ServiceResponse GetRequest(string url, string authorization);

        ServiceResponse DeleteRequest(string url, string authorization);

        ServiceResponse PostRequest(string url, string authorization, IDictionary<string, object> formPropertiesValues);
    }
}
