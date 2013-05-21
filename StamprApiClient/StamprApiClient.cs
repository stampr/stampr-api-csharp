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
using System.Web;
using StamprApiClient.Exceptions;
using System.Net;

namespace StamprApiClient
{
    public partial class StamprApiClient : IStamprApiClient
    {
        private const string _pingRelativeUrl = "test/ping";
        private const string _healthRelativeUrl = "health";
        private readonly IEnumerable<string> _allowedSchemas = new [] { "http", "https" };
        private readonly string _userName;
        private readonly string _password;
        private readonly Uri _baseUri;
        private readonly IServiceCommunicator _serviceCommunicator;
        private readonly JavaScriptSerializer _javaScriptSerializer;
        private IAuthorizationStrategy _authorizationStrategy;

        internal StamprApiClient(string baseUrl, string userName, string password, IServiceCommunicator serviceCommunicator, IAuthorizationStrategy authorizationStrategy)
        {
            _baseUri = new Uri(VirtualPathUtility.AppendTrailingSlash(baseUrl), UriKind.Absolute);
            if (!_allowedSchemas.Contains(_baseUri.Scheme))
            {
                throw new ArgumentException(string.Format(Resources.StringResource.NOT_VALID_URI, baseUrl));
            }

            _userName = userName;
            _password = password;
            _authorizationStrategy = authorizationStrategy;
            _serviceCommunicator = serviceCommunicator;
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public StamprApiClient(string baseUrl, string userName, string password, AuthorizationType authorizationType = AuthorizationType.Basic)
            : this(baseUrl, userName, password, new ServiceCommunicator(), SelectAuthorizationStrategy(userName, password, authorizationType))
        {
        }

        public object Ping()
        {
            Uri uri = new Uri(_baseUri, _pingRelativeUrl);
            ServiceResponse serviceReponse = _serviceCommunicator.GetRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            return serviceReponse.Response;
        }

        public string Health()
        {
            Uri uri = new Uri(_baseUri, _healthRelativeUrl);
            ServiceResponse serviceReponse = _serviceCommunicator.GetRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            return serviceReponse.Response;
        }

        private static IAuthorizationStrategy SelectAuthorizationStrategy(string username, string password, AuthorizationType authorizationType)
        {
            switch (authorizationType)
            {
                case AuthorizationType.Basic:
                default:
                    return new BasicAuthorizationStrategy(username, password);
            }
        }

        private T[] GetModels<T>(string relatedUri)
        {
            Uri uri = new Uri(_baseUri, relatedUri);
            ServiceResponse serviceReponse = _serviceCommunicator.GetRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            T[] resultBatchModel = _javaScriptSerializer.Deserialize<T[]>(serviceReponse.Response);
            return resultBatchModel;
        }

        private void ThrowCommunicationException(ServiceResponse serviceReponse)
        {
            ErrorDescription response = new ErrorDescription();
            if (!string.IsNullOrEmpty(serviceReponse.Response))
            {
                try
                {
                    response = _javaScriptSerializer.Deserialize<ErrorDescription>(serviceReponse.Response);
                }
                catch { }
            }

            throw new ServiceCommunicationException()
            {
                StatusCode = serviceReponse.StatusCode,
                Code = response.Code,
                Description = string.IsNullOrEmpty(response.Message) ? serviceReponse.Response : response.Message
            };
        }

        private string JoinRelativeUri(params object[] uriParts)
        {
            return string.Join("/", uriParts);
        }
    }
}
