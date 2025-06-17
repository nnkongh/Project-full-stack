using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebBlog.API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello from authorize");
        }
    }
}
