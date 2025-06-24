using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPhotoService _photoService;
        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IPhotoService photoService) {
            _userManager = userManager;
            _roleManager = roleManager;
            _photoService = photoService;
        }

        public async Task<AppUser?> GetUser(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);
            if(user == null) return null;
            return user;    
        }

        public async Task<AppUser?> UpdateProfile(UpdateProfileDto profile, ClaimsPrincipal userClaim)
        {
            var user = await _userManager.GetUserAsync(userClaim);
            if (user == null) return null;

            if (profile.profilePictureUrl != null && profile.profilePictureUrl.Length > 0)
            {
                var result = _photoService.AddPhotoAsync(profile.profilePictureUrl).Result;
                if (result.Error != null)
                {
                    throw new Exception(result.Error.Message);
                }
                user.ProfilePictureUrl = result.SecureUrl.ToString();
            }
            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}
    