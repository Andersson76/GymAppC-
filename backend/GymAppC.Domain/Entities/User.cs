using System.ComponentModel.DataAnnotations;

namespace GymAppC.Domain.Entities   
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required]
        public string Role { get; set; } = "User";
        public List<Workout> Workouts { get; set; } = new();
    }
}
