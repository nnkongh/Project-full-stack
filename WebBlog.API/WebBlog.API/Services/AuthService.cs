using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebBlog.API.DatabaseConnection;
using WebBlog.API.Interface;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private AppUser? _user;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthService(IConfiguration config, UserManager<AppUser> userManager, IEmailService emailService, RoleManager<IdentityRole> roleManager)

        {   _emailService = emailService;
            _roleManager = roleManager;
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
        }
        public async Task<TokenDto> CreateToken(AppUser user, bool populateExp)
        {
            //Create claim
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
            };
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }
            await _userManager.UpdateAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            // tôi muốn lưu trữ refresh token vào bảng user
            await _userManager.UpdateAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //Create credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //Create TokenDescriptor
            var tokenDescription = new SecurityTokenDescriptor
            {
                SigningCredentials = creds,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var accessToken = tokenHandler.WriteToken(token);
            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var tokenParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // không kiểm tra thời gian hết hạn của token 
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
        public void SetTokenInsideCookie(TokenDto token, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", token.AccessToken, new CookieOptions
            {
                HttpOnly = true, // để cookie chỉ có thể được truy cập từ máy chủ, không thể truy cập từ JavaScript
                Expires = DateTimeOffset.UtcNow.AddMinutes(15), // thời gian hết hạn của cookie
                IsEssential = true, // để cookie này được gửi trong mọi yêu cầu, kể cả khi không có người dùng đăng nhập
                SameSite = SameSiteMode.None, // để cookie có thể được gửi trong các yêu cầu cross-site
                Secure = true // chỉ gửi cookie qua HTTPS
            });
            context.Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });
        }

        public async Task<TokenDto> RefreshToken(TokenDto token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity?.Name!);
            if (user is null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            _user = user; // cập nhật user để lưu refresh token mới
            return await CreateToken(user, false); // false để không cập nhật thời gian hết hạn của refresh token mới 
        }

        public void RemoveTokenFromCookie(HttpContext context)
        {

            context.Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // đặt thời gian hết hạn của cookie là quá khứ để xóa nó
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            });
            context.Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(-1), // đặt thời gian hết hạn của cookie là quá khứ để xóa nó
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            });
        }

        public async Task ForgotPassword(ForgotPasswordDto forgot)
        {
            if (string.IsNullOrEmpty(forgot.ClientUri))
            {
                throw new ArgumentNullException(nameof(forgot.ClientUri), "Client URI cannot be null or empty.");
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(forgot.Email!);
                if (user == null)
                {
                    throw new ArgumentException("User not found with the provided email.", nameof(forgot.Email));
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var param = new Dictionary<string, string> { { "token", token }, { "email", forgot.Email! } };
                var callback = QueryHelpers.AddQueryString(forgot.ClientUri!, param);
                var message = new Message([forgot.Email!], "Reset password token", callback);
                _emailService.SendEmail(message);

            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while processing the forgot password request.", ex);
            }

        }

        public async Task ResetPassword(PasswordReset reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if(user == null)
            {
                throw new ArgumentException("User not found with the provided email.", nameof(reset.Email));
            }
            if(reset.Password != reset.ConfirmPassword)
            {
                throw new ArgumentException("Password and Confirm Password do not match.", nameof(reset));
            }
            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.Password);
            if(!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {errors}");
            }

        }


        public async Task Logout(ClaimsPrincipal principal, HttpContext context)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new ArgumentException("User ID not found in claims.", nameof(principal));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found with the provided ID.", nameof(userId));
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
            RemoveTokenFromCookie(context); // Xóa token khỏi cookie
        }

        public async Task Regiser(RegisterDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }
            var roleExists = await _roleManager.RoleExistsAsync("User");
            if (!roleExists)
            {
                var role = new IdentityRole("User");
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task Login(LoginDto model, HttpContext context)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }
            //Tạo oken khi đăng nhập thành công
            var tokenDto = await CreateToken(user, populateExp: true);
            SetTokenInsideCookie(tokenDto, context);
        }

        public async Task AssignRole(AssignRoleDto role)
        {
            var user = await _userManager.FindByNameAsync(role.UserName);
            if (user == null)
            {
                throw new ArgumentException("User not found with the provided username.", nameof(role.UserName));
            }
            var roleExists = await _roleManager.RoleExistsAsync(role.Role);
            if (!roleExists)
            {
                throw new ArgumentException("Role does not exist.", nameof(role.Role));
            }
            var result = await _userManager.AddToRoleAsync(user, role.Role);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign role: {errors}");
            }
        }

        public async Task AddRole(AddRoleDto role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role.Role);
            if (roleExists)
            {
                throw new ArgumentException("Role already exists.", nameof(role.Role));
            }
            var identityRole = new IdentityRole(role.Role);
            var result = await _roleManager.CreateAsync(identityRole);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create role: {errors}");
            }
        }
    }
}

