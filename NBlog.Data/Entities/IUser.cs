using NBlog.Data;

namespace NBlog.Data
{
    public interface IUser : IEntity
    {        
        string UserName { get; set; }
        string Name { get; set; }
        string PasswordHash { get; set; }
    }
}