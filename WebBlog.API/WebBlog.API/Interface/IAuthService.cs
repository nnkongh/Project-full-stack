using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Interface
{
    public interface IAuthService
    {
        Task<TokenDto> CreateToken(AppUser model, bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto token);
        void SetTokenInsideCookie(TokenDto token, HttpContext context);
        void RemoveTokenFromCookie(HttpContext context);
        Task ForgotPassword(ForgotPasswordDto forgot);
        Task ResetPassword(PasswordReset reset);
        Task Logout(ClaimsPrincipal principal, HttpContext context);
        Task Regiser(RegisterDto model);
        Task Login(LoginDto model, HttpContext context);
        Task AssignRole(AssignRoleDto role);
        Task AddRole(AddRoleDto role);
    }
}
