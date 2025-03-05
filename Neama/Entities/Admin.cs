namespace Ne3ma.Entities
{
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Store hashed password
        public string Role { get; set; } // SuperAdmin, Moderator, SupportAdmin
        public DateTime CreatedAt { get; set; }
    }
}
