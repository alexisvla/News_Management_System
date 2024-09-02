using Microsoft.Build.Framework;

namespace NEWS_WebAplication.Models.Account
{
    public class RegisterViewModels
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
        [Required]
        public string DisplayName { get; set; }
    }
}
