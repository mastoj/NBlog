using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySec.Hashing;
using NBlog.Data;
using NBlog.Data.Mongo;
using NBlog.Specs.Config;
using NBlog.Specs.Helpers;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class CommonSteps
    {
        private IHashGenerator _hashGenerator = new HashGenerator();

        [BeforeScenario("AdminUserExists")]
        public void AdminUserExists()
        {
            using (var userRepository = new MongoUserRepository(new MongoConfig()))
            {
                var user = new User
                               {
                                   Name = "Tomas",
                                   UserName = "admin",
                                   PasswordHash = _hashGenerator.GenerateHash("password")
                               };
                userRepository.Insert(user);
            }
        }

        [BeforeScenario("NotLoggedIn")]
        public void NotLoggedIn()
        {
            WebBrowser.Current.GoTo(Configuration.Host);
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink != null)
            {
                logOffLink.Click();
            }
        }

        [BeforeScenario("LoggedIn")]
        public void LoggedIn()
        {
            AdminUserExists();
            WebBrowser.Current.GoTo(Configuration.Host + NavigationHelper.Pages["start page"]);
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink != null)
            {
                logOffLink.Click();
            }
        }
    }
}
