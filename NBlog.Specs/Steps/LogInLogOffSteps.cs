﻿using System;
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
            using (var userRepository = new UserRepository(new MongoConfig()))
            {
                var users = userRepository.FindAll(y => y.UserName == userName);
                foreach (var user in users)
                {
                    userRepository.Delete(user);
                }
            }
            using (var userRepository = new UserRepository(new MongoConfig()))
            {
                var user = new CreateAdminModel()
                {
                    Name = name,
                    UserName = userName,
                    PasswordHash = _hashGenerator.GenerateHash(password)
                };
                var userDto = user.ToDTO();
                userRepository.Insert(userDto);
            }
        }

        [Given(@"it doesn't exist a user")]
        public void GivenItDoesnTExistAUser()
        {
            using (var userRepository = new UserRepository(new MongoConfig()))
            {
                userRepository.DeleteAll();
            }
        }
    }
}
