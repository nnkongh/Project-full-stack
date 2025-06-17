using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel.Dto
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Current Password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm New Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
