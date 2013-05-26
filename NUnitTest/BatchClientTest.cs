using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StamprApiClient.Authorization.Abstract;
using StamprApiClient.Authorization;
using Moq;
using StamprApiClient.Communicator.Abstract;
using StamprApiClient.Api.Models.Batch;
using StamprApiClient.Communicator.Models;
using StamprApiClient.Api;
using System.Text.RegularExpressions;
using StamprApiClient.Exceptions;
using StamprApiClient.Api.Models.Search;

namespace NUnitTest
{
    [TestFixture]
    public class BatchClientTest
    {
        private const string _basicAuthToken = "Basic ZHVtbXkudXNlckBleGFtcGxlLmNvbTpoZWxsbw==";
        private const string _username = "dummy.user@example.com";
        private const string _password = "hello";
        private const int _postDictionarySize = 3;
        private const string _url = "https://testing.dev.stam.pr/api";

        [Test]
        public void TestPostBatch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            BatchModel postModel = new BatchModel()
            {
                Config_Id = 4769,
                Status = Status.processing,
                Template = "Hello"
            };

            BatchModel model = stamprApiClient.CreateBatch(postModel);
            Assert.AreEqual(model.Config_Id, postModel.Config_Id);
            Assert.AreEqual(model.Status, postModel.Status);
            Assert.AreEqual(model.Template, postModel.Template);
            Assert.Greater(model.Batch_Id, 0);
            Assert.Greater(model.User_Id, 0);
        }

        [Test]
        public void TestModifyBatch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            bool result = stamprApiClient.ModifyBatch(1904, Status.processing);
            Assert.True(result);
        }

        [Test]
        public void TestModifyBatchUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.ModifyBatch(1904, Status.processing));
        }

        [Test]
        public void TestDeleteBatch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            bool result = stamprApiClient.DeleteBatch(1904);
            Assert.True(result);
        }

        [Test]
        public void TestDeleteBatchUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.DeleteBatch(1904));
        }

        [Test]
        public void TestPostBatchUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.CreateBatch(1,"", Status.hold));
        }

        [Test]
        public void TestPostBatchInvalidPostStatus()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ArgumentException>(() => stamprApiClient.CreateBatch(1, "", Status.archive));
        }

        [Test]
        public void TestGetBatchUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.GetBatches(4679));
        }

        [Test]
        public void TestGetBatchSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            BatchModel model = stamprApiClient.GetBatches(4679).First();
            Assert.AreEqual(model.Config_Id, 4679);
            Assert.AreEqual(model.Status, Status.processing);
            Assert.AreEqual(model.Template, "Hello");
            Assert.AreEqual(model.Batch_Id, 1904);
            Assert.AreEqual(model.User_Id, 1);
            Assert.AreEqual(model.Version, "1.0");
        }

        [Test]
        public void TestSearchBatchUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.GetBatches(Status.processing));
        }

        [Test]
        public void TestSearchBatchSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            BatchModel model = stamprApiClient.GetBatches(Status.processing).First();
            Assert.AreEqual(model.Config_Id, 4679);
            Assert.AreEqual(model.Status, Status.processing);
            Assert.AreEqual(model.Template, "Hello");
            Assert.AreEqual(model.Batch_Id, 1904);
            Assert.AreEqual(model.User_Id, 1);
            Assert.AreEqual(model.Version, "1.0");
        }

        [Test]
        public void TestSearchBatchInvalidSearch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ArgumentException>(() => stamprApiClient.GetBatches(new SearchModel<Status>()));
        }

        [Test]
        public void TestSearchBatchMailingsSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            var mailings = stamprApiClient.GetBatchMailings(1930, StamprApiClient.Api.Models.Mailing.Status.queued, DateTime.Parse("2013-05-24T18:01:35.707Z"), DateTime.Parse("2013-05-25T18:01:35.707Z"), 12).First();
            Assert.AreEqual(mailings.Format, StamprApiClient.Api.Models.Mailing.Format.none);
            Assert.AreEqual(mailings.Address, "Add");
            Assert.AreEqual(mailings.ReturnAddress, "RetAdd");
            Assert.AreEqual(mailings.Mailing_Id, 1348);
            Assert.AreEqual(mailings.User_Id, 1);
        }

        [Test]
        public void TestSearchBatchMailingsInvalidSearch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ArgumentException>(() => stamprApiClient.GetBatchMailings(1930, new SearchModel<StamprApiClient.Api.Models.Mailing.Status>()));
        }
        
        private IAuthorizationStrategy MockAuthStrategy()
        {
            Mock<IAuthorizationStrategy> mock = new Mock<IAuthorizationStrategy>();
            mock.Setup(x => x.GetAuthorizationHeader()).Returns(_basicAuthToken);
            return mock.Object;
        }

        private IServiceCommunicator MockServiceCommunicator()
        {
            Mock<IServiceCommunicator> mock = new Mock<IServiceCommunicator>();
            mock.Setup(x => 
                x.PostRequest(
                It.Is<string>(address => ComparePostAddress(address)),
                It.Is<string>(header =>
                    string.Compare(header, _basicAuthToken) == 0),
                It.Is<IDictionary<string, object>>(dict => IsValidBatchDictionary(dict))
                )).Returns<string, string, IDictionary<string, object>>((url, header, dictionary) => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = BuildResponse(dictionary)
                });
            mock.Setup(x => 
                x.PostRequest(
                It.Is<string>(address => VerifyAddressWithId(address)),
                It.Is<string>(header =>
                    string.Compare(header, _basicAuthToken) == 0),
                It.IsAny<IDictionary<string, object>>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = "true"
                });
            mock.Setup(x =>
                x.PostRequest(
                It.Is<string>(address => !ComparePostAddress(address) && !VerifyAddressWithId(address)),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, object>>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Status = "Not Found",
                    Response = string.Empty
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => !VerifyAddressWithQueryString(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Status = "Not Found",
                    Response = string.Empty
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => VerifyAddressWithId(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = GetBatchString()
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => VerifyAddressWithQueryString(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = GetBatchString()
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => VerifyAddressWithBatchMailingQueryString(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = GetMailingString()
                });
            mock.Setup(x =>
                x.DeleteRequest(
                It.Is<string>(address => VerifyAddressWithId(address)),
                It.Is<string>(header =>
                    string.Compare(header, _basicAuthToken) == 0))).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = "true"
                });
            mock.Setup(x =>
                x.DeleteRequest(
                It.Is<string>(address => !VerifyAddressWithId(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Status = "Not Found",
                    Response = string.Empty
                });
            return mock.Object;
        }

        private bool ComparePostAddress(string address)
        {
            return string.Compare(address, string.Concat(_url, "/batches")) == 0;
        }

        private bool VerifyAddressWithId(string address)
        {
            string configsString = string.Concat(_url, "/batches/");
            if (!address.StartsWith(configsString))
            {
                return false;
            }

            int id;
            string idString = address.Replace(configsString, string.Empty);
            return int.TryParse(idString, out id);
        }

        private bool VerifyAddressWithQueryString(string address)
        {
            string configsString = string.Concat(_url, "/batches/with/", Status.processing.ToString());
            return address.StartsWith(configsString);
        }

        private bool VerifyAddressWithBatchMailingQueryString(string address)
        {
            return address == "https://testing.dev.stam.pr/api/batches/1930/mailings/with/queued/2013-05-24T18%3a01%3a35.707Z/2013-05-25T18%3a01%3a35.707Z/12";
        }

        private bool IsValidBatchDictionary(IDictionary<string, object> dictionary)
        {
            bool isOfValidSize = dictionary.Count == _postDictionarySize;
            Status status;
            bool isValidBatchStatus = dictionary.ContainsKey("status") && Enum.TryParse<Status>(dictionary["status"].ToString(), false, out status);
            int config_id;
            bool isValidBatchConfigId = dictionary.ContainsKey("config_id") && int.TryParse(dictionary["config_id"].ToString(), out config_id);
            bool isValidBatchTemplate = dictionary.ContainsKey("template");
            return isOfValidSize && isValidBatchStatus && isValidBatchConfigId && isValidBatchTemplate;
        }

        private string BuildResponse(IDictionary<string, object> dictionary)
        {
            string format = "\"{0}\": {1}";
            IList<string> allProperties = new List<string>(dictionary.Select(pair =>
                string.Format(format, pair.Key, pair.Value is string || pair.Value is Enum ? string.Concat("\"", pair.Value, "\"") : pair.Value)));
            Random random = new Random();
            allProperties.Add(string.Format(format, "user_id", random.Next(1, int.MaxValue)));
            allProperties.Add(string.Format(format, "batch_id", random.Next(1, int.MaxValue)));
            string responseProperties = string.Concat("{",string.Join(string.Concat(",", Environment.NewLine), allProperties), "}");
            return responseProperties;
        }

        private string GetBatchString()
        {
            return "[{\"version\":\"1.0\",\"template\":\"Hello\",\"user_id\":1,\"config_id\":4679,\"status\":\"processing\",\"batch_id\":1904}]";
        }

        private string GetMailingString()
        {
            return "[{\"batch_id\":\"1919\",\"address\":\"Add\",\"returnaddress\":\"RetAdd\",\"format\":\"none\",\"data\":\"{\\\"Hello\\\":\\\"bye\\\"}\",\"user_id\":1,\"printer_id\":null,\"pdf\":null,\"status\":\"render\",\"initiated\":\"2013-05-21T18:01:35.707Z\",\"mailing_id\":1348}]";
        }
    }
}
