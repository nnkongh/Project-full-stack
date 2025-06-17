using WebBlog.API.Models;

namespace WebBlog.API.ViewModel.Dto
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
