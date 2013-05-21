using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StamprApiClient.Authorization.Abstract;
using StamprApiClient.Authorization;
using Moq;
using StamprApiClient.Communicator.Abstract;
using StamprApiClient.Api.Models.Config;
using StamprApiClient.Communicator.Models;
using StamprApiClient.Api;
using System.Text.RegularExpressions;
using StamprApiClient.Exceptions;

namespace NUnitTest
{
    [TestFixture]
    public class ConfigClientTest
    {
        private const string _basicAuthToken = "Basic ZHVtbXkudXNlckBleGFtcGxlLmNvbTpoZWxsbw==";
        private const string _username = "dummy.user@example.com";
        private const string _password = "hello";
        private const int _dictionarySize = 5;
        private const string _url = "https://testing.dev.stam.pr/api";

        [Test]
        public void TestPostConfig()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            ConfigModel model = stamprApiClient.CreateConfig();
            Assert.AreEqual(model.Output, Output.single);
            Assert.AreEqual(model.Style, Style.color);
            Assert.AreEqual(model.Turnaround, Turnaround.threeday);
            Assert.AreEqual(model.Size, Size.standard);
            Assert.AreEqual(model.ReturnEnvelope, false);
            Assert.Greater(model.Config_Id, 0);
            Assert.Greater(model.User_Id, 0);
        }

        [Test]
        public void TestPostConfigUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.CreateConfig());
        }

        [Test]
        public void TestGetConfigUnsuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient("http://no", _username, _password, serviceCommunicator, basicAuthStrategy);
            Assert.Throws<ServiceCommunicationException>(() => stamprApiClient.GetConfig(1));
        }

        [Test]
        public void TestGetConfigSuccessful()
        {
            IAuthorizationStrategy basicAuthStrategy = MockAuthStrategy();
            IServiceCommunicator serviceCommunicator = MockServiceCommunicator();
            IStamprApiClient stamprApiClient = new StamprApiClient.StamprApiClient(_url, _username, _password, serviceCommunicator, basicAuthStrategy);
            ConfigModel model = stamprApiClient.GetConfig(4679).First();
            Assert.AreEqual(model.Output, Output.single);
            Assert.AreEqual(model.Style, Style.color);
            Assert.AreEqual(model.Turnaround, Turnaround.threeday);
            Assert.AreEqual(model.Size, Size.standard);
            Assert.AreEqual(model.ReturnEnvelope, false);
            Assert.AreEqual(model.Config_Id, 4679);
            Assert.AreEqual(model.User_Id, 1);
            Assert.AreEqual(model.Version, "1.0");
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
                It.Is<IDictionary<string, object>>(dict => IsValidConfigDictionary(dict))
                )).Returns<string, string, IDictionary<string, object>>((url, header, dictionary) => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = BuildResponse(dictionary)
                });
            mock.Setup(x => 
                x.PostRequest(
                It.Is<string>(address => !ComparePostAddress(address)),
                It.IsAny<string>(),
                It.IsAny<IDictionary<string, object>>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Status = "Not Found",
                    Response = string.Empty
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => !VerifyGetAddress(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Status = "Not Found",
                    Response = string.Empty
                });
            mock.Setup(x =>
                x.GetRequest(
                It.Is<string>(address => VerifyGetAddress(address)),
                It.IsAny<string>())).Returns(() => new ServiceResponse()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Status = "OK",
                    Response = GetConfigString()
                });
            return mock.Object;
        }

        private bool ComparePostAddress(string address)
        {
            return string.Compare(address, string.Concat(_url, "/configs")) == 0;
        }

        private bool VerifyGetAddress(string address)
        {
            string configsString = string.Concat(_url, "/configs/");
            if (!address.StartsWith(configsString))
            {
                return false;
            }

            int id;
            string idString = address.Replace(configsString, string.Empty);
            return int.TryParse(idString, out id);
        }

        private bool IsValidConfigDictionary(IDictionary<string, object> dictionary)
        {
            bool isOfValidSize = dictionary.Count == _dictionarySize;
            Size size;
            bool isValidConfigSize = dictionary.ContainsKey("size") && Enum.TryParse<Size>(dictionary["size"].ToString(), false, out size);
            Turnaround turnaround;
            bool isValidConfigTurnaround = dictionary.ContainsKey("turnaround") && Enum.TryParse<Turnaround>(dictionary["turnaround"].ToString(), false, out turnaround);
            Style style;
            bool isValidConfigStyle = dictionary.ContainsKey("style") && Enum.TryParse<Style>(dictionary["style"].ToString(), false, out style);
            Output output;
            bool isValidConfigOutput = dictionary.ContainsKey("output") && Enum.TryParse<Output>(dictionary["output"].ToString(), false, out output);
            bool returnenvelope;
            bool isValidConfigReturnEnvelope = dictionary.ContainsKey("returnenvelope") && bool.TryParse(dictionary["returnenvelope"].ToString(), out returnenvelope) && returnenvelope.ToString().ToLower() == dictionary["returnenvelope"] as string;
            return isOfValidSize && isValidConfigSize && isValidConfigTurnaround && isValidConfigStyle && isValidConfigOutput && isValidConfigReturnEnvelope;
        }

        private string BuildResponse(IDictionary<string, object> dictionary)
        {
            string format = "\"{0}\": {1}";
            IList<string> allProperties = new List<string>(dictionary.Select(pair =>
                string.Format(format, pair.Key, pair.Value is string || pair.Value is Enum ? string.Concat("\"", pair.Value, "\"") : pair.Value)));
            Random random = new Random();
            allProperties.Add(string.Format(format, "user_id", random.Next(1, int.MaxValue)));
            allProperties.Add(string.Format(format, "config_id", random.Next(1, int.MaxValue)));
            string responseProperties = string.Concat("{",string.Join(string.Concat(",", Environment.NewLine), allProperties), "}");
            return responseProperties;
        }

        private string GetConfigString()
        {
            return "[ {\"version\": \"1.0\", \"size\": \"standard\",  \"turnaround\": \"threeday\",  \"style\": \"color\",  \"output\": \"single\",  \"returnenvelope\": false,  \"user_id\": 1,  \"config_id\": 4679 }]";
        }
    }
}
