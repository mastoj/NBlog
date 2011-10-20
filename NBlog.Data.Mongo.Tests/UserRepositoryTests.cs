using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NBlog.Data.DTO;
using NBlog.Data.Mongo.Repositories;
using NUnit.Framework;

namespace NBlog.Data.Mongo.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {

        [Test]
        public void InsertOneUserInDb_ShouldResultInOneUserInDb()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();

            // act
            InsertUser(user);
            using (var userRepo = GetTestRepository())
            {
                var createdUser = userRepo.Single(y => y.UserName == user.UserName);

                // assert
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(user.PasswordHash, createdUser.PasswordHash);
                Assert.AreNotEqual(Guid.Empty, createdUser.Id);
                Assert.AreEqual(user.Name, createdUser.Name);
            }
        }

        private static void DeleteAllUsers()
        {
            using (var userRepo = GetTestRepository())
            {
                userRepo.DeleteAll();
            }            
        }

        private static UserRepository GetTestRepository()
        {
            var connectionString = GetConnectionString();
            return new UserRepository(new MongoConfiguration() { ConnectionString = connectionString });
        }

        private static string GetConnectionString()
        {
            var connectionString = "mongodb://localhost/test";
            if (ConfigurationManager.AppSettings["Environment"] == "Test")
            {
                connectionString = "mongodb://tester:TestPass@staff.mongohq.com:10075/NBlogTest";
            }
            return connectionString;
        }

        private static void InsertUser(User user)
        {
            using (var userRepo = GetTestRepository())
            {
                userRepo.Insert(user);
            }
        }

        private static User CreateUser(string name = "tomas", string passwordHash = "p1p1", string userName = "uname")
        {
            var user = new User() { Name = name, PasswordHash = passwordHash, UserName = userName };
            return user;
        }

        [Test]
        public void UpdateUser_ResultsInUpdatedUser()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();
            InsertUser(user);
            User updatedUser;

            // act
            using (var userRepo = GetTestRepository())
            {
                updatedUser = userRepo.Single(y => y.UserName == user.UserName);
                updatedUser.PasswordHash = "p2p2";
            }

            // assert
            Assert.AreNotEqual(user.PasswordHash, updatedUser.PasswordHash);
            Assert.AreEqual("p2p2", updatedUser.PasswordHash);
        }

        [Test]
        public void AllUser_ReturnsAllUser()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();
            var user2 = CreateUser(userName: "uname2");
            InsertUser(user);
            InsertUser(user2);
            IList<User> users;

            // act
            using (var userRepo = GetTestRepository())
            {
                users = userRepo.All().ToList();
            }

            // assert
            Assert.AreEqual(2, users.Count);
        }

        [Test]
        public void AllUserWithPredicate_ReturnsCorrectUsers()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();
            var user2 = CreateUser(userName: "uname2");
            var user3 = CreateUser(userName: "3uname2");
            InsertUser(user);
            InsertUser(user2);
            InsertUser(user3);
            IList<User> users;

            // act
            using (var userRepo = GetTestRepository())
            {
                users = userRepo.FindAll(y => y.UserName.StartsWith("uname")).ToList();
            }

            // assert
            Assert.AreEqual(2, users.Count);
        }

        [Test]
        public void DeleteUser_DeletesUser()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();
            var user2 = CreateUser(userName: "uname2");
            var user3 = CreateUser(userName: "3uname2");
            InsertUser(user);
            InsertUser(user2);
            InsertUser(user3);
            int count;

            // act
            using (var userRepo = GetTestRepository())
            {
                userRepo.DeleteAll();
                count = userRepo.All().Count();
            }

            // assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public void DeleteAll_DeletesAllUser()
        {
            // arrange
            DeleteAllUsers();
            var user = CreateUser();
            InsertUser(user);
            int count = 1;

            // act
            using (var userRepo = GetTestRepository())
            {
                userRepo.DeleteAll();
                count = userRepo.All().Count();
            }

            // assert
            Assert.AreEqual(0, count);
        }
    }
}
