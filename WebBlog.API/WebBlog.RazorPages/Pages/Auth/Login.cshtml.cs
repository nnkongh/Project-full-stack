using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Auth
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsLoginPage"] = true;
        }
    }
}
