using Neama.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Neama.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailOrPhone(ErrorMessage = "Please provide a valid email address or phone number.")]
        public string EmailOrPhone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
