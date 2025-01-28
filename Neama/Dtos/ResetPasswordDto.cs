using Neama.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Neama.Dtos
{
    public class ResetPasswordDto
    {
        //[Required(ErrorMessage ="password is Required")]
        //public string NewPassword { get; set; }
        //[Compare("NewPassword",ErrorMessage ="The password do not match .")]
        //public string ConfirmPassword { get; set; }
        //[Required]
        //[EmailOrPhone(ErrorMessage = "Please provide a valid email address or phone number.")]
        //public string EmailOrPhone { get; set; }
        //public string Email { get; set; }
        //public string Token { get; set; }
        [Required]
        [EmailOrPhone(ErrorMessage = "Please provide a valid email address or phone number.")]
        public string EmailOrPhone { get; set; }
        [Required(ErrorMessage = "password is Required")]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "The password do not match .")]
        public string ConfirmPassword { get; set; }

    }

}
