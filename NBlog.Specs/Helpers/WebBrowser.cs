using System;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Helpers
{
    public static class WebBrowser
    {
        private static ObservableBrowser _current;

        public static ObservableBrowser Current
        {
            get
            {
                _current = _current ?? new ObservableBrowser();
                return _current;
            }
        }
    }
}