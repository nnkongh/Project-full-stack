using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAuthService _token;
        private readonly IEmailService _emailSender;
        private readonly ILogger<AuthenController> _logger;

        public AuthenController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IAuthService tokenService,IEmailService emailSender, ILogger<AuthenController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _token = tokenService;
            _emailSender = emailSender;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync("User");
                if(!roleExists)
                {
                    var role = new IdentityRole("User");
                    await _roleManager.CreateAsync(role);
                }
                await _userManager.AddToRoleAsync(user, "User");
                return Ok("User created successfully");
            }
            else
            {
                return StatusCode(500, result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid username or password");
            }   

            var tokenDto = await _token.CreateToken(user, populateExp: true);
            _token.SetTokenInsideCookie(tokenDto, HttpContext);
            return Ok(new {message = "Hello"});
        }
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(forgot.ClientUri))
            {
                throw new ArgumentNullException(nameof(forgot.ClientUri), "Client URI cannot be null or empty.");
            }
            try
            {
                _logger.LogWarning($"Log: {forgot.ClientUri}");
                var user = await _userManager.FindByEmailAsync(forgot.Email!);
                _logger.LogWarning($"User with email {forgot.Email} requested password reset.");
                if (user == null)
                {
                    return NotFound(user);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var param = new Dictionary<string, string>
            {
                { "token",token },
                {"email",forgot.Email }

            };
                var callback = QueryHelpers.AddQueryString(forgot.ClientUri!, param);
                _logger.LogInformation($"Password reset link: {callback}");
                var message = new Message([user.Email], "Reset password token", callback);
                _emailSender.SendEmail(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the forgot password request.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
           
            return Ok();
        }
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(PasswordReset reset)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
            {
                return BadRequest(user);
            }
            if (reset.Password != reset.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match");
            }
            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.Password);
            if(result.Succeeded)
            {
                return Ok("Password reset successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
            
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDto assign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(assign.UserName);
            if(user == null)
            {
                return BadRequest("User not found");
            }
            var roleExists = await _roleManager.RoleExistsAsync(assign.Role);
            if(!roleExists)
            {
                return BadRequest("Role not found");
            }
            var result = await _userManager.AddToRoleAsync(user, assign.Role);
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Role assigned successfully");
        }
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(AddRoleDto addRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var roleExists = await _roleManager.RoleExistsAsync(addRole.Role);
            if (roleExists)
            {
                return BadRequest("Role already exists");
            }
            var role = new IdentityRole(addRole.Role);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role created successfully");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
