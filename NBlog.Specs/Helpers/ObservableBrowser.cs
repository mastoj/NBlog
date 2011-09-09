using System.Net;
using WatiN.Core;

namespace NBlog.Specs.Helpers
{
    public class ObservableBrowser : IE
    {
        private NavigationObserver _observer;

        public ObservableBrowser()
        {
            _observer = new NavigationObserver(this);
        }

        public void ShouldHave(HttpStatusCode expectedStatusCode)
        {
            _observer.ShouldHave(expectedStatusCode);
        }
    }
}