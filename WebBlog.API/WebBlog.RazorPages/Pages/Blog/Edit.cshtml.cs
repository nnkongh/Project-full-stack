﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebBlog.RazorPages.Pages.Blog
{
    public class EditModel : PageModel
    {
        public void OnGet()
        {
            ViewData["IsBlogPage"] = true;
            ViewData["IsLoginPage"] = true;
        }
    }
}