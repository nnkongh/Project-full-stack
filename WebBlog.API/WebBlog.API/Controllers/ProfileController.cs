using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.Services;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        public ProfileController(IUserService userService, IPhotoService photoService)
        {
            _userService = userService;
            _photoService = photoService;
        }
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUser(User);
            if(user == null)
            {
                return BadRequest("User not found");
            }
            var userDto = new ProfileDto
            {
                UserName = user.UserName!,
                Email = user.Email!,
                Role = user.Role ?? "User",
                ProfilePicture = user.ProfilePictureUrl
            };
            return Ok(userDto);
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.UpdateProfile(profile, User);
            return Ok(new { profilePictureUrl =  user!.ProfilePictureUrl});
        }
    }
}
