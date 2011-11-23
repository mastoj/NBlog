using System;

namespace NBlog.Domain.Entities
{
    [Serializable]
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
