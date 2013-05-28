using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StamprApiClient.Authorization.Abstract;
using StamprApiClient.Authorization;
using Moq;
using StamprApiClient.Communicator.Abstract;
using StamprApiClient.Api.Models.Mailing;
using StamprApiClient.Communicator.Models;
using StamprApiClient.Api;
using System.Text.RegularExpressions;
using StamprApiClient.Exceptions;
using StamprApiClient.Api.Models.Search;

namespace NUnitTest
{
    [TestFixture]
    public class MailingClientTest
    {
        private const string _basicAuthToken = "Basic ZHVtbXkudXNlckBleGFtcGxlLmNvbTpoZWxsbw==";
        private const string _username = "dummy.user@example.com";
        private const string _password = "hello";
        private const int _postDictionarySize = 4;
        private const string _url = "https://testing.dev.stam.pr/api";

        [Test]
        public void TestPostMailing()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            MailingModel postModel = new MailingModel()
            {
                Batch_Id = 4769,
                Format = Format.none,
                Address = "Add",
                ReturnAddress = "RetAdd"
            };

            MailingModel model = stamprApiClient.CreateMailing(postModel);
            Assert.AreEqual(model.Batch_Id, postModel.Batch_Id);
            Assert.AreEqual(model.Format, postModel.Format);
            Assert.AreEqual(model.Address, postModel.Address);
            Assert.AreEqual(model.ReturnAddress, postModel.ReturnAddress);
            Assert.Greater(model.Mailing_Id, 0);
            Assert.Greater(model.User_Id, 0);
        }

        [Test]
        public void TestDeleteMailing()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            bool result = stamprApiClient.DeleteMailing(1904);
            Assert.True(result);
        }

        [Test]
        public void TestDeleteMailingUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.DeleteMailing(1904));
        }

        [Test]
        public void TestPostMailingUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.CreateMailing(1, "Add", "RetAdd", Format.none));
        }

        [Test]
        public void TestGetMailingUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.GetMailings(4679));
        }

        [Test]
        public void TestGetMailingSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            MailingModel model = stamprApiClient.GetMailings(1348).First();
            Assert.AreEqual(model.Format, Format.none);
            Assert.AreEqual(model.Address, "Add");
            Assert.AreEqual(model.ReturnAddress, "RetAdd");
            Assert.AreEqual(model.Mailing_Id, 1348);
            Assert.AreEqual(model.User_Id, 1);
        }

        [Test]
        public void TestSearchMailingUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.GetMailings(Status.processing));
        }

        [Test]
        public void TestSearchMailingSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            MailingModel model = stamprApiClient.GetMailings(Status.render).First();
            Assert.AreEqual(model.Format, Format.none);
            Assert.AreEqual(model.Address, "Add");
            Assert.AreEqual(model.ReturnAddress, "RetAdd");
            Assert.AreEqual(model.Mailing_Id, 1348);
            Assert.AreEqual(model.User_Id, 1);
        }

        [Test]
        public void TestSearchMailingInvalidSearch()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ArgumentException>(() => stamprApiClient.GetMailings(new SearchModel<Status>()));
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
                It.Is<IDictionary<string, object>>(dict => IsValidMailingDictionary(dict))
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
                It.Is<string>(address => VerifyAddressWithQueryString(address)),
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
            return string.Compare(address, string.Concat(_url, "/mailings")) == 0;
        }

        private bool VerifyAddressWithId(string address)
        {
            string configsString = string.Concat(_url, "/mailings/");
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
            string configsString = string.Concat(_url, "/mailings/");
            return address.StartsWith(configsString);
        }

        private bool IsValidMailingDictionary(IDictionary<string, object> dictionary)
        {
            
            Format format;
            bool isValidMailingFormat = dictionary.ContainsKey("format") && Enum.TryParse<Format>(dictionary["format"].ToString(), false, out format);
            int batch_id;
            bool isValidMailingBatchId = dictionary.ContainsKey("batch_id") && int.TryParse(dictionary["batch_id"].ToString(), out batch_id);
            bool isValidMailingAddress = dictionary.ContainsKey("address");
            bool isValidMailingReturnAddress = dictionary.ContainsKey("returnaddress");
            int postDicSize = _postDictionarySize;
            if (dictionary.ContainsKey("data"))
            {
                postDicSize++;
            }

            if (dictionary.ContainsKey("md5"))
            {
                postDicSize++;
            }

            bool isOfValidSize = dictionary.Count == postDicSize;
            return isOfValidSize && isValidMailingFormat && isValidMailingBatchId && isValidMailingAddress && isValidMailingReturnAddress;
        }

        private string BuildResponse(IDictionary<string, object> dictionary)
        {
            string format = "\"{0}\": {1}";
            IList<string> allProperties = new List<string>(dictionary.Select(pair =>
                string.Format(format, pair.Key, pair.Value is string || pair.Value is Enum ? string.Concat("\"", pair.Value, "\"") : pair.Value)));
            Random random = new Random();
            allProperties.Add(string.Format(format, "user_id", random.Next(1, int.MaxValue)));
            allProperties.Add(string.Format(format, "mailing_id", random.Next(1, int.MaxValue)));
            string responseProperties = string.Concat("{", string.Join(string.Concat(",", Environment.NewLine), allProperties), "}");
            return responseProperties;
        }

        private string GetMailingString()
        {
            return "[{\"batch_id\":\"1919\",\"address\":\"Add\",\"returnaddress\":\"RetAdd\",\"format\":\"none\",\"data\":\"{\\\"Hello\\\":\\\"bye\\\"}\",\"user_id\":1,\"printer_id\":null,\"pdf\":null,\"status\":\"render\",\"initiated\":\"2013-05-21T18:01:35.707Z\",\"mailing_id\":1348}]";
        }
    }
}
