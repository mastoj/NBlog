using NBlog.Domain;
using NBlog.Domain.Entities;

namespace NBlog.Translators
{
    public static class UserTranslator
    {
        public static User ToDTO(this IUser user)
        {
            var userDTO = new User()
                              {
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
                              Name = user.Name,
                              PasswordHash = user.PasswordHash,
                              UserName = user.UserName
                          };
            return iuser;
        }
    }
}
