using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBlog.API.ViewModel.Dto;

namespace WebBlog.RazorPages.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsBlogPage"] = true;
            ViewData["IsLoginPage"] = true;
        }
    }
}
