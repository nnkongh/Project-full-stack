using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel.Dto
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
