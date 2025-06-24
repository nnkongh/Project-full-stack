using System.Runtime.CompilerServices;
using WebBlog.API.Models;
using WebBlog.API.ViewModel.Comment;

namespace WebBlog.API.Mapper
{
    public static class CommentMapper
    {
        public static CommentDTO MapComment(this Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                BlogId = comment.BlogId,
                UserName = comment.User?.UserName, // Assuming Blog has a UserId property
            };
        }
        public static Comment MapCreateComment(this CreateCommentDTO cmtDTO, int blogId, string userId)
        {
            return new Comment
            {
                Content = cmtDTO.Content,
                BlogId = blogId,
                UserId = userId, // Assuming CreateCommentDTO has a UserId property
                CreatedAt = DateTime.UtcNow // Set CreatedAt to current time
            };
        }
        public static Comment MapUpdateComment(this UpdateCommentDTO cmtDTO)
        {
            return new Comment
            {
                Content = cmtDTO.Content,
            };
        }
    }
}
