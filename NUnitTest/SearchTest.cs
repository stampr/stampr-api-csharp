using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using StamprApiClient.Api.Models.Search;
using StamprApiClient.Resources;
using StamprApiClient.Api.Models.Batch;

namespace NUnitTest
{
    [TestFixture]
    public class SearchBatchTest
    {
        [Test]
        public void TestSearchValidAll()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = DateTime.Now,
                Start = DateTime.Now,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidNoStatus()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                End = DateTime.Now,
                Start = DateTime.Now,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidNoStatusNoPaging()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                End = DateTime.Now,
                Start = DateTime.Now
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidNoPaging()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = DateTime.Now,
                Start = DateTime.Now
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidNoPagingAndEndTime()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                Start = DateTime.Now
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidStartTime()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Start = DateTime.Now
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchValidStatus()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
        }

        [Test]
        public void TestSearchMissingStartTime()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = DateTime.Now
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.False(res.IsValid);
            Assert.AreEqual(StringResource.START_VALUE_SHOULD_BE_SPECIFIED, res.Description);
        }

        [Test]
        public void TestSearchEmpty()
        {
            SearchModel<Status> model = new SearchModel<Status>();
            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.False(res.IsValid);
            Assert.AreEqual(StringResource.NO_VALUES_TO_SEARCH, res.Description);
        }

        [Test]
        public void TestSearchMissingEndTimeForPaging()
        {
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Start = DateTime.Now,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.False(res.IsValid);
            Assert.AreEqual(StringResource.START_END_VALUES_SHOULD_BE_SPECIFIED, res.Description);
        }
    }
}
