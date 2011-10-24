using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TJ.Extensions;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class PostSteps
    {
        [Then(@"I should find a list of posts with one entry")]
        public void ThenIShouldFindAListWithOneEntry()
        {
            var list = WebBrowser.Current.Table(Find.ByClass("posts"));
            if (list.Exists.IsFalse())
            {
                Assert.Fail("Can't find post list");
            }
        }

        [Then(@"it contains the string ""(.*)""")]
        public void ThenItContainsTheStringDemoTitle(string stringToLookFor)
        {
            var stringExist = WebBrowser.Current.ContainsText(stringToLookFor);
            if (stringExist.IsFalse())
            {
                Assert.Fail("Couldn't find the text {0}", stringToLookFor);
            }
        }
    }
}
