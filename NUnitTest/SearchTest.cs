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
        private const string _dateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

        [Test]
        public void TestSearchValidAll()
        {
            DateTime date = DateTime.Now;

            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = date.AddMinutes(5),
                Start = date,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("with", searchArray[0]);
            Assert.AreEqual(Status.hold.ToString(), searchArray[1]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[2]);
            Assert.AreEqual(date.ToUniversalTime().AddMinutes(5).ToString(_dateFormat), searchArray[3]);
            Assert.AreEqual("10", searchArray[4]);
        }

        [Test]
        public void TestSearchValidNoStatus()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                End = date.AddMinutes(5),
                Start = date,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("browse", searchArray[0]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[1]);
            Assert.AreEqual(date.ToUniversalTime().AddMinutes(5).ToString(_dateFormat), searchArray[2]);
            Assert.AreEqual("10", searchArray[3]);
        }

        [Test]
        public void TestSearchValidNoStatusNoPaging()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                End = date.AddMinutes(5),
                Start = date
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("browse", searchArray[0]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[1]);
            Assert.AreEqual(date.ToUniversalTime().AddMinutes(5).ToString(_dateFormat), searchArray[2]);
        }

        [Test]
        public void TestSearchValidNoPaging()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = date.AddMinutes(5),
                Start = date
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("with", searchArray[0]);
            Assert.AreEqual(Status.hold.ToString(), searchArray[1]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[2]);
            Assert.AreEqual(date.ToUniversalTime().AddMinutes(5).ToString(_dateFormat), searchArray[3]);
        }

        [Test]
        public void TestSearchValidNoPagingAndEndTime()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                Start = date
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("with", searchArray[0]);
            Assert.AreEqual(Status.hold.ToString(), searchArray[1]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[2]);
        }

        [Test]
        public void TestSearchValidStartTime()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Start = date
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.True(res.IsValid);
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("browse", searchArray[0]);
            Assert.AreEqual(date.ToUniversalTime().ToString(_dateFormat), searchArray[1]);

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
            string[] searchArray = model.PropertiesToSearch();
            Assert.AreEqual("with", searchArray[0]);
            Assert.AreEqual(Status.hold.ToString(), searchArray[1]);
        }

        [Test]
        public void TestSearchMissingStartTime()
        {
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Status = Status.hold,
                End = date.AddMinutes(5)
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
            DateTime date = DateTime.Now;
            SearchModel<Status> model = new SearchModel<Status>()
            {
                Start = date,
                Paging = 10
            };

            SearchValidationResult res = model.IsValidSearchCombination();
            Assert.False(res.IsValid);
            Assert.AreEqual(StringResource.START_END_VALUES_SHOULD_BE_SPECIFIED, res.Description);
        }
    }
}
