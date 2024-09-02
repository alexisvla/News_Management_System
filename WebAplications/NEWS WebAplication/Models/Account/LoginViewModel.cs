using Microsoft.Build.Framework;

namespace NEWS_WebAplication.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string? ErrorMessage { get; private set; }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        internal void serErrorMessage(string v)
        {
            ErrorMessage = v;
        }
    }
}
