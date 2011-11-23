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
using NBlog.Models;
using NBlog.Specs.Config;
using NBlog.Specs.Helpers;
using NBlog.Specs.Infrastructure;
using NBlog.Translators;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class LogInLogOffSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am logged in as the admin user")]
        public void GivenIAmLoggedInAsTheAdminUser()
        {
            var user = new User() {Id = Guid.NewGuid(), Name = "Tomas", UserName = "admin", PasswordHash = "asdf1234"};
            UserHelper.InsertUser(user);
            WebBrowser.Current.GoTo(Config.Configuration.Host + NavigationHelper.Pages["login page"]);
            var logOffLink = WebBrowser.Current.Links.SingleOrDefault(y => y.Id == "logOff");
            if (logOffLink == null)
            {
                var formSteps = new FormSteps();
                var table = new Table("InputField", "Input");
                table.AddRow("UserName", user.UserName);
                table.AddRow("Password", user.PasswordHash);
                formSteps.WhenEnterTheFollowingInformation(table);
                formSteps.WhenIClickTheButton("log in");
            }
        }

        [Given(@"it exist an account with the credentials")]
        public void GivenItExistAnAccountWithTheCredentials(Table table)
        {
            var userName = table.Rows[0]["UserName"];
            var password = table.Rows[0]["Password"];
            var name = table.Rows[0]["Name"];
            UserRepositoryHelper.InsertUser(userName, password, name);
        }

        [Given(@"it doesn't exist a user")]
        public void GivenItDoesnTExistAUser()
        {
            UserRepositoryHelper.DeleteAll();
        }
    }
}
