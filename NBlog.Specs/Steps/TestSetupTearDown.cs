using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Specs.Helpers;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class TestSetupTearDown
    {
        [AfterTestRun]
        public static void CleanUp()
        {
            WebBrowser.Current.Close();
        }
    }
}
