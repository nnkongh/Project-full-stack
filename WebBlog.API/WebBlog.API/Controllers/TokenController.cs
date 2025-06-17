using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebBlog.API.Interface;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]TokenDto token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var tokenRef = await _authService.RefreshToken(token);
            return Ok(tokenRef);
        }
    }
}
