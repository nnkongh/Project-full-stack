using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientUri { get; set; } 
    }
}
