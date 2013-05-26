using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Api;
using StamprApiClient.Communicator.Abstract;
using StamprApiClient.Communicator;
using StamprApiClient.Communicator.Models;
using System.Web.Script.Serialization;
using StamprApiClient.Api.Models.Batch;
using StamprApiClient.Resources;
using StamprApiClient.Api.Models.Search;
using StamprApiClient.Api.Models.Mailing;
using StamprApiClient.Exceptions;
using System.Net;
using Status = StamprApiClient.Api.Models.Batch.Status;
using MailingStatus = StamprApiClient.Api.Models.Mailing.Status;

namespace StamprApiClient
{
    public partial class StamprApiClient : IStamprApiClient
    {
        private const string _batchRelatedUri = "batches";
        private readonly IEnumerable<Status> _creationAllowedStatuses = new[] { Status.hold, Status.processing };

        public BatchModel CreateBatch(BatchModel batch)
        {
            if (!_creationAllowedStatuses.Contains(batch.Status))
            {
                throw new ArgumentException(string.Format(StringResource.STATUS_NOT_ALLOWED, batch.Status));
            }

            IDictionary<string, object> propertyValues = batch.ToPostPropertiesDictionary();
            Uri uri = new Uri(_baseUri, _batchRelatedUri);
            ServiceResponse serviceReponse = _serviceCommunicator.PostRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader(), propertyValues);
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            BatchModel resultConfigModel =_modelConvertor.ConvertToModel<BatchModel>(serviceReponse.Response);
            return resultConfigModel;
        }

        public BatchModel CreateBatch(int configId, string template, Status status)
        {
            BatchModel batch = new BatchModel()
            {
                Config_Id = configId,
                Status = status,
                Template = template
            };

            return CreateBatch(batch);
        }

        public bool ModifyBatch(BatchModel batch)
        {
            IDictionary<string, object> propertyValues = batch.ToUpdatePropertiesDictionary();
            Uri uri = new Uri(_baseUri, JoinRelativeUri(_batchRelatedUri, batch.Batch_Id));
            ServiceResponse serviceReponse = _serviceCommunicator.PostRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader(), propertyValues);
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            bool result;
            bool canParse = bool.TryParse(serviceReponse.Response, out result);
            return canParse && result;
        }

        public bool ModifyBatch(int batchId, Status status)
        {
            BatchModel batch = new BatchModel()
            {
                Status = status,
                Batch_Id = batchId
            };

            return ModifyBatch(batch);
        }

        public bool DeleteBatch(int batchId)
        {
            Uri uri = new Uri(_baseUri, JoinRelativeUri(_batchRelatedUri, batchId));
            ServiceResponse serviceReponse = _serviceCommunicator.DeleteRequest(uri.ToString(), _authorizationStrategy.GetAuthorizationHeader());
            if (serviceReponse.StatusCode != HttpStatusCode.OK)
            {
                ThrowCommunicationException(serviceReponse);
            }

            bool result;
            bool canParse = bool.TryParse(serviceReponse.Response, out result);
            return canParse && result;
        }

        public BatchModel[] GetBatches(int id)
        {
            string relatedUri = JoinRelativeUri(_batchRelatedUri, id);
            return GetModels<BatchModel>(relatedUri);
        }

        public BatchModel[] GetBatches(Status status)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(Status status, DateTime start)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(Status status, DateTime start, DateTime end)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start,
                End = end
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(Status status, DateTime start, DateTime end, int paging)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Status = status,
                Start = start,
                End = end,
                Paging = paging
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(DateTime start)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(DateTime start, DateTime end)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start,
                End = end
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(DateTime start, DateTime end, int paging)
        {
            SearchModel<Status> searchModel = new SearchModel<Status>()
            {
                Start = start,
                End = end,
                Paging = paging
            };

            return GetBatches(searchModel);
        }

        public BatchModel[] GetBatches(SearchModel<Status> searchModel)
        {
            List<string> props = new List<string>();
            props.Add(_batchRelatedUri);
            props.AddRange(searchModel.PropertiesToSearch());
            string relatedUri = JoinRelativeUri(props.ToArray());
            return GetModels<BatchModel>(relatedUri);
        }

        public MailingModel[] GetBatchMailings(int id)
        {
            string relatedUri = JoinRelativeUri(_batchRelatedUri, id, _mailingRelatedUri);
            return GetMailingModels<MailingModel>(relatedUri);
        }

        public MailingModel[] GetBatchMailings(int id, MailingStatus status)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Status = status
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Status = status,
                Start = start
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start, DateTime end)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Status = status,
                Start = start,
                End = end
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start, DateTime end, int paging)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Status = status,
                Start = start,
                End = end,
                Paging = paging
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, DateTime start)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Start = start
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, DateTime start, DateTime end)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Start = start,
                End = end
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, DateTime start, DateTime end, int paging)
        {
            SearchModel<MailingStatus> searchModel = new SearchModel<MailingStatus>()
            {
                Start = start,
                End = end,
                Paging = paging
            };

            return GetBatchMailings(id, searchModel);
        }

        public MailingModel[] GetBatchMailings(int id, SearchModel<MailingStatus> searchModel)
        {
            List<string> props = new List<string>();
            props.Add(_batchRelatedUri);
            props.Add(id.ToString());
            props.Add(_mailingRelatedUri);
            props.AddRange(searchModel.PropertiesToSearch());
            string relatedUri = JoinRelativeUri(props.ToArray());
            return GetMailingModels<MailingModel>(relatedUri);
        }
    }
}
