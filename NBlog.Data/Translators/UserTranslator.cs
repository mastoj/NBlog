using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Entities;

namespace NBlog.Domain.Translators
{
    public static class UserTranslator
    {
        public static User ToDTO(this IUser user)
        {
            var userDTO = new User()
                              {
                                  Id = user.Id,
                                  Name = user.Name,
                                  PasswordHash = user.PasswordHash,
                                  UserName = user.UserName
                              };
            return userDTO;
        }

        public static T ToIUser<T>(this User user) where T : IUser, new()
        {
            T iuser = new T()
                          {
                              Id = user.Id,
                              Name = user.Name,
                              PasswordHash = user.PasswordHash,
                              UserName = user.UserName
                          };
            return iuser;
        }
    }
}
