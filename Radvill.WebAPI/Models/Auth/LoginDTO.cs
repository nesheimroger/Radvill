using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Auth
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}