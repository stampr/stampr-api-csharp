using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StamprApiClient.Authorization.Abstract;
using StamprApiClient.Authorization;

namespace NUnitTest
{
    [TestFixture]
    public class AuthorizationStrategyTest
    {
        private const string _basicAuthToken = "Basic ZHVtbXkudXNlckBleGFtcGxlLmNvbTpoZWxsbw==";
        private const string _username = "dummy.user@example.com";
        private const string _password = "hello";

        [Test]
        public void TestBasicAuthStrategy()
        {
            IAuthorizationStrategy basicAuthStrategy = new BasicAuthorizationStrategy(_username, _password);
            string header = basicAuthStrategy.GetAuthorizationHeader();
            Assert.AreEqual(_basicAuthToken, header);
        }
    }
}
