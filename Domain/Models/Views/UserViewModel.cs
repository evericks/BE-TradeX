namespace Domain.Models.Views
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreateAt { get; set; }
    }
}
