using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailSender;
        private readonly ILogger<AuthenController> _logger;

        public AuthenController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IAuthService tokenService, IEmailService emailSender, ILogger<AuthenController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = tokenService;
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.Regiser(model);
            return Ok("User registered successfully");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.Login(model, HttpContext);
            return Ok();
        }
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _authService.ForgotPassword(forgot);
            return Ok();
        }
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(PasswordReset reset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _authService.ResetPassword(reset);
            return Ok("Password reset successfully");
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(AssignRoleDto assign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _authService.AssignRole(assign);
            return Ok("Role assigned successfully");
        }
        [HttpPost("add-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(AddRoleDto addRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _authService.AddRole(addRole);
            return Ok("Role added successfully");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(User, HttpContext);
            return Ok("Logged out successfully");

        }
    }
}
