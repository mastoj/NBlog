using DeleporterCore.Client;
using NBlog.Specs.Helpers;
using NBlog.Specs.Infrastructure;
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
