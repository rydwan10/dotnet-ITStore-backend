using System.ComponentModel.DataAnnotations;

namespace ITStore.DTOs.Users
{
    public class UserCredentialsDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
