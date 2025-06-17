using System.ComponentModel.DataAnnotations;

namespace WebBlog.API.ViewModel.Dto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
