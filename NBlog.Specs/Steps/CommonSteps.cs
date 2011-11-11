using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySec.Hashing;
using NBlog.Domain;
using NBlog.Domain.Mongo;
using NBlog.Domain.Mongo.Repositories;
using NBlog.Domain.Translators;
using NBlog.Models;
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
            using (var userRepository = new UserRepository(new MongoConfig()))
            {
                userRepository.DeleteAll();
            }
            using (var userRepository = new UserRepository(new MongoConfig()))
            {
                var user = new CreateAdminModel
                               {
                                   Name = "Tomas",
                                   UserName = "admin",
                                   PasswordHash = _hashGenerator.GenerateHash("asdf1234")
                               };
                var userDto = user.ToDTO();
                userRepository.Insert(userDto);
            }
        }

        [BeforeScenario("NotLoggedIn")]
        public void NotLoggedIn()
        {
            WebBrowser.Current.GoTo(Config.Configuration.Host);
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
            WebBrowser.Current.GoTo(Config.Configuration.Host + NavigationHelper.Pages["login page"]);
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink == null)
            {
                var formSteps = new FormSteps();
                var table = new Table("InputField", "Input");
                table.AddRow("UserName", "admin");
                table.AddRow("Password", "asdf1234");
                formSteps.WhenEnterTheFollowingInformation(table);
                formSteps.WhenIClickTheButton("log in");
            }
        }
    }
}
