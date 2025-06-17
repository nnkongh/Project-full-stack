namespace WebBlog.API.Helper
{
    public class QueryObject
    {
        public string? Tags { get; set; } = null;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;

    }
}
