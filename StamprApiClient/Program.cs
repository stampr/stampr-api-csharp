using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Api;
using StamprApiClient.Api.Models.Config;
using StamprApiClient.Api.Models.Batch;
using StamprApiClient.Api.Models.Mailing;
using StamprApiClient.Api.Models.Search;
using Status = StamprApiClient.Api.Models.Batch.Status;
using MailingStatus = StamprApiClient.Api.Models.Mailing.Status;

namespace StamprApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //IStamprApiClient stamprApiClient = new StamprApiClient("https://testing.dev.stam.pr/api", "dummy.user@example.com", "hello");
            ////var res1 = stamprApiClient.Health();
            //// var ers1 = stamprApiClient.Ping();
            ////ConfigModel result = stamprApiClient.CreateConfig();
            ////var batch = stamprApiClient.CreateBatch(4679, "Hello", Status.hold);
            ////stamprApiClient.ModifyBatch(batch.Batch_Id, Status.processing);
            ////var batches1 = stamprApiClient.GetBatches(batch.Batch_Id);
            ////var res = stamprApiClient.DeleteBatch(batch.Batch_Id);
            ////var batches2 = stamprApiClient.GetBatches(DateTime.Now.AddHours(-3));
            ////stamprApiClient.ModifyBatch(1897, 4721, Status.processing);
            ////MailingModel mailing = stamprApiClient.CreateMailing(batch.Batch_Id, "Add", "RetAdd", Format.none, "Hello");
            //stamprApiClient.GetMailings(DateTime.Parse("2013-05-21T18:01:35.707Z"));
            //ConfigModel[] result1 = stamprApiClient.GetConfig(result.Config_Id);
            Console.ReadKey();
        }
    }
}
