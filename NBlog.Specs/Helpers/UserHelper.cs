using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeleporterCore.Client;
using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;
using NBlog.Specs.Infrastructure;
using NBlog.Tests;

namespace NBlog.Specs.Helpers
{
    public static class UserHelper
    {
        public static void InsertUser(User user)
        {
            Deleporter.Run(
                () =>
                {
                    var inMemoryUserRepository = new InMemoryUserRepository();
                    //var userToInsert = new User()
                    //                       {
                    //                           Id = user.Id,
                    //                           Name = user.Name,
                    //                           PasswordHash = user.PasswordHash,
                    //                           UserName = user.UserName
                    //                       };
                    inMemoryUserRepository.Insert(user);
                    DeleporterMvcUtils.TemporarilyReplaceBinding<IUserRepository>(inMemoryUserRepository);
                });
        }
    }
}
