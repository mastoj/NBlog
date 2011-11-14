using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySec.Hashing;
using NBlog.Domain;
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
