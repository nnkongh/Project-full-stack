using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;


namespace WebBlog.API.Models
{
    public class AppUser : IdentityUser
    {
        public string Role { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
