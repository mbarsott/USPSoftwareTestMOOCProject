using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace Mbarsott.JsonHelper
{
    public class JsonHelperTests
    {
        [OneTimeSetUp]
        public void InitializeTracking()
        {
            for (char c = 'A'; c <= 'T'; c++ )
                JsonHelper.Tracker.Add(c.ToString(), 0);
        }

        [TearDown]
        public void DisplayTrackingResults()
        {
            foreach (var kv in JsonHelper.Tracker.AsEnumerable())
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }

        [TestCaseSource(nameof(PropertiesAndOccurrencesCount))]
        public void FindNodesByProperyNameFindsRightNumberOfNodes(string propertyName, int expectedCount)
        {
            var tokens = JsonHelper.FindNodesByPropertyAndValue(JObject.Parse(JSON_STRING), propertyName, "");

            Assert.That(tokens.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void FindNodesByProperyNameWithNullParamsThrowsArgumentNullException()
        {
            Assert.That(JsonHelper.FindNodesByPropertyAndValue(null, "test", "test").Count() == 0);
            Assert.That(JsonHelper.FindNodesByPropertyAndValue(JObject.Parse(JSON_STRING), null, "test").Count() == 0);
            Assert.That(JsonHelper.FindNodesByPropertyAndValue(JObject.Parse(JSON_STRING), string.Empty, "test").Count() == 0);
        }

        private const string JSON_STRING = 
            @"{
                'short': 
                {
                    'original': 'http://www.foo.com/',
                    'short': 'krehqk',
                    'error': 
                    {
                        'code': 0,
                        'error': 'another error property',
                        'short': 'another short property',
                        'msg': 'No action taken'
                    }
                }
            }";

        private static IEnumerable PropertiesAndOccurrencesCount
        {
            get
            {
                yield return new TestCaseData("short", 3);
                yield return new TestCaseData("original", 1);
                yield return new TestCaseData("error", 2);
                yield return new TestCaseData("code", 1);
                yield return new TestCaseData("msg", 1);
                yield return new TestCaseData("zero", 0);
            }
        }
    }
}
