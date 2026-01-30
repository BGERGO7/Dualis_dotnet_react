namespace React_dotnet.database.Models
{
    public class Role
    {
        public long Id { get; set; }
        public required string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
