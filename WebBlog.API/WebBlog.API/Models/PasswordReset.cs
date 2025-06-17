using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.Models
{
    public class PasswordReset
    {
        public string Token { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
