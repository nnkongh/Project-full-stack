using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Auth
{
    public class ResetPasswordModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsLoginPage"] = true;
        }
    }
}
