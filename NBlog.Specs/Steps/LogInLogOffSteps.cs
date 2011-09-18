using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class LogInLogOffSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am not logged in")]
        public void GivenIAmNotLoggedIn()
        {
            When("I am on the login page");
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink != null)
            {
                logOffLink.Click();
            }
        }
    
        [Given(@"it exist an account with the credentials")]
        public void GivenItExistAnAccountWithTheCredentials(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"it doesn't exist a user")]
        public void GivenItDoesnTExistAUser()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
