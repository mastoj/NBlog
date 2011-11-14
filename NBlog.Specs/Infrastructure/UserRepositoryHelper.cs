using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DeleporterCore.Client;
using EasySec.Hashing;
using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;
using NBlog.Tests;
using TJ.Extensions;

namespace NBlog.Specs.Infrastructure
{
    public static class UserRepositoryHelper
    {
        public static void InsertUser(string userName, string password, string name)
        {
            Deleporter.Run(
                () =>
                {
                    IUserRepository repository =
                        DependencyResolver.Current.GetService<IUserRepository>() as InMemoryUserRepository;
                    if (repository.IsNull())
                    {
                        repository = new InMemoryUserRepository();
                        DeleporterMvcUtils.TemporarilyReplaceBinding(repository);
                    }
                    repository.Insert(new User
                                          {
                                              Name = name,
                                              PasswordHash = password,
                                              UserName = userName
                                          });
                });
        }

        public static void DeleteAll()
        {
            Deleporter.Run(
                () =>
                {
                    IUserRepository repository = new InMemoryUserRepository();
                    DeleporterMvcUtils.TemporarilyReplaceBinding(repository);
                });
        }
    }
}
