using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Authorization.Abstract;

namespace StamprApiClient.Authorization
{
    internal class BasicAuthorizationStrategy : IAuthorizationStrategy
    {
        private readonly string _userName;
        private readonly string _password;

        internal BasicAuthorizationStrategy(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public string GetAuthorizationHeader()
        {
            string credentialsString = string.Format("{0}:{1}", _userName, _password);
            byte[] toBytes = Encoding.UTF8.GetBytes(credentialsString);
            string encodedValue = Convert.ToBase64String(toBytes);
            return string.Format("Basic {0}", encodedValue);
        }
    }
}
