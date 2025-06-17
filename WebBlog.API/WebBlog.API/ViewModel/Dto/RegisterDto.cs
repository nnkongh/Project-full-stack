using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Compare("ConfirmPassword", ErrorMessage = "Password and Confirm Password do not match")]
        public string Password { get; set; }



        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
