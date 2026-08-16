using GymAppC.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace GymAppC.Domain.Entities   
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        [Required]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [Required]
        [MaxLength(32)]
        public string Role { get; set; } = AppRoles.User;
        public List<Workout> Workouts { get; set; } = new();
    }
}
