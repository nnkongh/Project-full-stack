﻿namespace WebBlog.API.ViewModel.Comment
{
    public class CreateCommentDTO
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
