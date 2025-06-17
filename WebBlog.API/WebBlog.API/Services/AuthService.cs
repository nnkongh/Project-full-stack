using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        public AuthService(IConfiguration config, UserManager<AppUser> userManager)
        {
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
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
            if(jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
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
            if(user is null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
            _user = user; // cập nhật user để lưu refresh token mới
            return await CreateToken(user, false); // false để không cập nhật thời gian hết hạn của refresh token mới 
        }
    }
}
