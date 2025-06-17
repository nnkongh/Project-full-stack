using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Interface
{
    public interface IAuthService
    {
        Task<TokenDto> CreateToken(AppUser model, bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto token);
        void SetTokenInsideCookie(TokenDto token, HttpContext context);
    }
}
