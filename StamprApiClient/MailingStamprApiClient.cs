using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Api;
using System.Web.Script.Serialization;
using StamprApiClient.Resources;
using StamprApiClient.Api.Models.Mailing;
using StamprApiClient.Communicator.Models;
using StamprApiClient.Api.Models.Search;
using System.Net;
using StamprApiClient.Exceptions;

namespace StamprApiClient
{
    public partial class StamprApiClient : IStamprApiClient
    {
        private const string _mailingRelatedUri = "mailings";

        public MailingModel CreateMailing(MailingModel mailing)
        {
            IDictionary<string, object> propertyValues = mailing.ToPostPropertiesDictionary();
            Uri relUri = new Uri(_mailingRelatedUri,UriKind.Relative);
            Uri uri = new Uri(_baseUri, relUri);
            ServiceResponse serviceReponse = _serviceCommunicator.PostRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader(), propertyValues);
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            MailingModel resultConfigModel = _mailingModelConvertor.ConvertToModel<MailingModel>(serviceReponse.Response);
            return resultConfigModel;
        }

        public MailingModel CreateMailing(int batchId, string address, string returnAddress, Format format, IDictionary<string, string> data = null)
        {
            MailingModel mailing = new MailingModel()
            {
                Batch_Id = batchId,
                Address = address,
                ReturnAddress = returnAddress,
                Format = format,
                Data = data
            };

            return CreateMailing(mailing);
        }

        public bool DeleteMailing(int mailingId)
        {
            Uri uri = new Uri(_baseUri, JoinRelativeUri(_mailingRelatedUri, mailingId));
            ServiceResponse serviceReponse = _serviceCommunicator.DeleteRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            bool result;
            bool canParse = bool.TryParse(serviceReponse.Response, out result);
            return canParse && result;
        }

        public MailingModel[] GetMailings(int id)
        {
            string relatedUri = JoinRelativeUri(_mailingRelatedUri, id);
            return GetMailingModels<MailingModel>(relatedUri);
        }

        public MailingModel[] GetMailings(Status status)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(Status status, DateTime start)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(Status status, DateTime start, DateTime end)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start,
                End = end
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(Status status, DateTime start, DateTime end, int paging)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start,
                End = end,
                Paging = paging
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(DateTime start)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(DateTime start, DateTime end)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start,
                End = end
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(DateTime start, DateTime end, int paging)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start,
                End = end,
                Paging = paging
            };

            return GetMailings(searchModel);
        }

        public MailingModel[] GetMailings(SearchModel<Status> searchModel)
        {
            List<string> props = new List<string>();
            props.Add(_mailingRelatedUri);
            props.AddRange(searchModel.PropertiesToSearch());
            string relatedUri = JoinRelativeUri(props.ToArray());
            return GetMailingModels<MailingModel>(relatedUri);
        }

        private T[] GetMailingModels<T>(string relatedUri)
        {
            Uri uri = new Uri(_baseUri, relatedUri);
            ServiceResponse serviceReponse = _serviceCommunicator.GetRequest(uri.AbsoluteUri, _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            T[] resultBatchModel = _mailingModelConvertor.ConvertToModel<T[]>(serviceReponse.Response);
            return resultBatchModel;
        }
    }
}
