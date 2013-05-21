using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StamprApiClient.Resources;

namespace StamprApiClient.Api.Models.Search
{
    public class SearchModel<T> where T : struct
    {
        private const string _dateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        public T? Status { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Paging { get; set; }

        public SearchValidationResult IsValidSearchCombination()
        {
            if (End.HasValue && !Start.HasValue)
            {
                return new SearchValidationResult()
                {
                    IsValid = false,
                    Description = StringResource.START_VALUE_SHOULD_BE_SPECIFIED
                };                
            }

            if (Paging.HasValue && (!End.HasValue || !Start.HasValue))
            {
                return new SearchValidationResult()
                {
                    IsValid = false,
                    Description = StringResource.START_END_VALUES_SHOULD_BE_SPECIFIED
                };
            }

            if (!Status.HasValue && !Start.HasValue)
            {
                return new SearchValidationResult()
                {
                    IsValid = false,
                    Description = StringResource.NO_VALUES_TO_SEARCH
                };
            }

            return new SearchValidationResult()
            {
                IsValid = true
            };
        }

        internal string[] PropertiesToSearch()
        {
            SearchValidationResult result = IsValidSearchCombination();
            if (!result.IsValid)
            {
                throw new ArgumentException(result.Description);
            }

            IList<string> objects = new List<string>();
            if (Status.HasValue)
            {
                objects.Add(Status.Value.ToString());
            }

            if (Start.HasValue)
            {
                objects.Add(Start.Value.ToUniversalTime().ToString(_dateFormat));
            }

            if (End.HasValue)
            {
                objects.Add(End.Value.ToUniversalTime().ToString(_dateFormat));
            }

            if (Paging.HasValue)
            {
                objects.Add(Paging.Value.ToString());
            }

            return objects.ToArray();
        }
    }
}
