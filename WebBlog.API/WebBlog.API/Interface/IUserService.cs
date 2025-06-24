using System.Security.Claims;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Interface
{
    public interface IUserService
    {
        Task<AppUser?> GetUser(ClaimsPrincipal user);
        Task<AppUser?> UpdateProfile(UpdateProfileDto profile, ClaimsPrincipal user);


    }
}
