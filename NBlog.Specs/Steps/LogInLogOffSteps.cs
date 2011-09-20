using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySec.Hashing;
using NBlog.Data;
using NBlog.Data.Mongo;
using NBlog.Specs.Config;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class LogInLogOffSteps : TechTalk.SpecFlow.Steps
    {
        private IHashGenerator _hashGenerator = new HashGenerator();

        [Given(@"it exist an account with the credentials")]
        public void GivenItExistAnAccountWithTheCredentials(Table table)
        {
            var userName = table.Rows[0]["UserName"];
            var password = table.Rows[0]["Password"];
            var name = table.Rows[0]["Name"];
            using (var userRepository = new MongoUserRepository(new MongoConfig()))
            {
                var user = new User
                {
                    Name = name,
                    UserName = userName,
                    PasswordHash = _hashGenerator.GenerateHash(password)
                };
                userRepository.Insert(user);
            }
        }

        [Given(@"it doesn't exist a user")]
        public void GivenItDoesnTExistAUser()
        {
            using (var userRepository = new MongoUserRepository(new MongoConfig()))
            {
                userRepository.DeleteAll();
            }
        }
    }
}
