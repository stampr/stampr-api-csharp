using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Api;
using StamprApiClient.Authorization.Abstract;
using StamprApiClient.Api.Models;
using StamprApiClient.Authorization;
using StamprApiClient.Api.Models.Config;
using StamprApiClient.Communicator.Abstract;
using StamprApiClient.Communicator;
using StamprApiClient.Communicator.Models;
using System.Web.Script.Serialization;
using System.Net;
using StamprApiClient.Exceptions;

namespace StamprApiClient
{
    public partial class StamprApiClient : IStamprApiClient
    {
        private const string _configRelatedUri = "configs";

        public ConfigModel CreateConfig()
        {
            ConfigModel configModel = new ConfigModel
            {
                Style = Style.color,
                Output = Output.single,
                Turnaround = Turnaround.threeday,
                Size = Size.standard,
                ReturnEnvelope = false
            };

            IDictionary<string, object> propertyValues = configModel.ToPostPropertiesDictionary();
            Uri uri = new Uri(_baseUri, _configRelatedUri);
            ServiceResponse serviceReponse = _serviceCommunicator.PostRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader(), propertyValues);
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            ConfigModel resultConfigModel = new JavaScriptSerializer().Deserialize<ConfigModel>(serviceReponse.Response);
            return resultConfigModel;
        }

        public ConfigModel[] GetConfig(int id)
        {
            string relatedUri = JoinRelativeUri(_configRelatedUri, id);
            return GetModels<ConfigModel>(relatedUri);
        }

        public ConfigModel[] GetAllConfigs()
        {
            string relatedUri = JoinRelativeUri(_configRelatedUri, "all");
            return GetModels<ConfigModel>(relatedUri);
        }

        public ConfigModel[] GetAllConfigs(int paging)
        {
            string relatedUri = JoinRelativeUri(_configRelatedUri, "all", paging);
            return GetModels<ConfigModel>(relatedUri);
        }
    }
}
