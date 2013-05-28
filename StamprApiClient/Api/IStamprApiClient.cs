using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Api.Models.Config;
using StamprApiClient.Api.Models.Batch;
using StamprApiClient.Api.Models.Mailing;
using StamprApiClient.Api.Models.Search;
using Status = StamprApiClient.Api.Models.Batch.Status;
using MailingStatus = StamprApiClient.Api.Models.Mailing.Status;

namespace StamprApiClient.Api
{
    interface IStamprApiClient
    {
        ConfigModel CreateConfig();

        ConfigModel[] GetConfig(int id);

        ConfigModel[] GetAllConfigs();

        ConfigModel[] GetAllConfigs(int paging = 0);

        BatchModel CreateBatch(BatchModel batch);

        BatchModel CreateBatch(int configId, string template, Status status);

        bool ModifyBatch(BatchModel batch);

        bool ModifyBatch(int batchId, Status status);

        bool DeleteBatch(int batchId);

        BatchModel[] GetBatches(int id);

        BatchModel[] GetBatches(Status status);

        BatchModel[] GetBatches(Status status, DateTime start);

        BatchModel[] GetBatches(Status status, DateTime start, DateTime end);

        BatchModel[] GetBatches(Status status, DateTime start, DateTime end, int paging);

        BatchModel[] GetBatches(DateTime start);

        BatchModel[] GetBatches(DateTime start, DateTime end);

        BatchModel[] GetBatches(DateTime start, DateTime end, int paging);

        BatchModel[] GetBatches(SearchModel<Status> searchModel);

        MailingModel[] GetBatchMailings(int id);

        MailingModel[] GetBatchMailings(int id, MailingStatus status);

        MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start);

        MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start, DateTime end);

        MailingModel[] GetBatchMailings(int id, MailingStatus status, DateTime start, DateTime end, int paging);

        MailingModel[] GetBatchMailings(int id, DateTime start);

        MailingModel[] GetBatchMailings(int id, DateTime start, DateTime end);

        MailingModel[] GetBatchMailings(int id, DateTime start, DateTime end, int paging);

        MailingModel[] GetBatchMailings(int id, SearchModel<MailingStatus> searchModel);

        MailingModel CreateMailing(MailingModel mailing);

        MailingModel CreateMailing(int batchId, string address, string returnAddress, Format format, IDictionary<string, string> data = null);

        bool DeleteMailing(int mailingId);

        MailingModel[] GetMailings(int id);

        MailingModel[] GetMailings(MailingStatus status);

        MailingModel[] GetMailings(MailingStatus status, DateTime start);

        MailingModel[] GetMailings(MailingStatus status, DateTime start, DateTime end);

        MailingModel[] GetMailings(MailingStatus status, DateTime start, DateTime end, int paging);

        MailingModel[] GetMailings(DateTime start);

        MailingModel[] GetMailings(DateTime start, DateTime end);

        MailingModel[] GetMailings(DateTime start, DateTime end, int paging);

        MailingModel[] GetMailings(SearchModel<MailingStatus> searchModel);

        string Health();

        object Ping();
    }
}
