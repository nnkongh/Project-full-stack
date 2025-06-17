using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsLoginPage"] = true;
        }
    }
}
