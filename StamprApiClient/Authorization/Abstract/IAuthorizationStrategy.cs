using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StamprApiClient.Authorization.Abstract
{
    internal interface IAuthorizationStrategy
    {
        string GetAuthorizationHeader();
    }
}
