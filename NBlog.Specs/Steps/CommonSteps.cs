using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeleporterCore.Client;
using EasySec.Hashing;
using NBlog.Domain;
using NBlog.Domain.Entities;
using NBlog.Domain.Mongo;
using NBlog.Domain.Mongo.Repositories;
using NBlog.Domain.Repositories;
using NBlog.Models;
using NBlog.Specs.Config;
using NBlog.Specs.Helpers;
using NBlog.Specs.Infrastructure;
using NBlog.Tests;
using NBlog.Translators;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class CommonSteps
    {
        [BeforeScenario("AdminUserExists")]
        public void AdminUserExists()
        {
            Deleporter.Run(
                () =>
                {
                    var inMemoryUserRepository = new InMemoryUserRepository();
                    var user = new User
                    {
                        Id = Guid.Empty,
                        Name = "Tomas",
                        UserName = "admin",
                        PasswordHash = "asdf1234"
                    };
                    inMemoryUserRepository.Insert(user);
                    DeleporterMvcUtils.TemporarilyReplaceBinding<IUserRepository>(inMemoryUserRepository);
                });
        }

        [BeforeScenario("NotAuthenticated")]
        public void NotLoggedIn()
        {
            WebBrowser.Current.GoTo(Config.Configuration.Host);
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink != null)
            {
                logOffLink.Click();
            }
        }

        [BeforeScenario("Authenticated")]
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

        [BeforeScenario]
        public void CommonSetup()
        {
            Deleporter.Run(
                () =>
                    {
                        IHashGenerator dumbHashGenerator = new DumbHashGenerator();
                        DeleporterMvcUtils.TemporarilyReplaceBinding(dumbHashGenerator);
                    });
        }

        [AfterScenario]
        public void CommonCleanUp()
        {
            Deleporter.Run(TidyupUtils.PerformTidyup);
        }
    }
}
