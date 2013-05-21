using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StamprApiClient.Api.Models.Search
{
    public class SearchValidationResult
    {
        internal SearchValidationResult() { }

        public string Description { get; internal set; }
        public bool IsValid { get; internal set; }
    }
}
