﻿using WebBlog.API.Models;

namespace WebBlog.API.Repository
{
    public interface ICommentRepository 
    {
        Task<List<Comment?>> GetByBlogIdAsync(int blogId);
        Task<Comment> CreateAsync(Comment cmt);
        Task<Comment?> UpdateAsync(Comment cmt, int id);
        Task<Comment?> DeleteAsync(int id);
    }
}
