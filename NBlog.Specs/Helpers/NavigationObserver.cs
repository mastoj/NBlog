using System.Globalization;
using System.Net;
using NUnit.Framework;
using SHDocVw;
using WatiN.Core;

namespace NBlog.Specs.Helpers
{
    public class NavigationObserver
    {
        private HttpStatusCode _statusCode;
        private bool _errorOccured;

        public NavigationObserver(IE ie)
        {
            InternetExplorer internetExplorer = (InternetExplorer)ie.InternetExplorer; 
            internetExplorer.NavigateError += IeNavigateError;
            internetExplorer.NavigateComplete2 += internetExplorer_NavigateComplete2;
        }

        void internetExplorer_NavigateComplete2(object pDisp, ref object URL)
        {
            if (!_errorOccured)
            {
                _statusCode = HttpStatusCode.OK;
            }
            _errorOccured = false;
        } 

        public void ShouldHave(HttpStatusCode expectedStatusCode)
        {
            if (!_statusCode.Equals(expectedStatusCode))
            {
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Wrong status code. Expected {0}, but was {1}", expectedStatusCode, _statusCode));
            }
        } 
        private void IeNavigateError(object pDisp, ref object URL, ref object Frame, ref object StatusCode, ref bool Cancel)
        {
            _errorOccured = true;
            _statusCode = (HttpStatusCode)StatusCode; 
        }
    }
}