using NBlog.Domain;

namespace NBlog.Domain
{
    public interface IUser : IEntity
    {        
        string UserName { get; set; }
        string Name { get; set; }
        string PasswordHash { get; set; }
    }
}