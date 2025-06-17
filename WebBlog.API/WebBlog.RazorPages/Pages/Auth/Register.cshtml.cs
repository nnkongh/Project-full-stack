using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsLoginPage"] = true;
        }
    }
}
