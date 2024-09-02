using System.ComponentModel.DataAnnotations;

namespace NEWS_WebAplication.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
