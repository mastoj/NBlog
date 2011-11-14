namespace NBlog.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
