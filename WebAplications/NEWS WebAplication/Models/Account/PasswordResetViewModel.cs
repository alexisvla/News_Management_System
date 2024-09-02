using System.ComponentModel.DataAnnotations;

namespace NEWS_WebAplication.Models.Account
{
    public class PasswordResetViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set;}
        
        [Required]
        public string Token { get; set; }
    }
}
