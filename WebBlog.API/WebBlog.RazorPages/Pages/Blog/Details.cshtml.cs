﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Blog
{
    public class DetailsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsBlogPage"] = true;
            ViewData["IsLoginPage"] = true;
        }
    }
}
