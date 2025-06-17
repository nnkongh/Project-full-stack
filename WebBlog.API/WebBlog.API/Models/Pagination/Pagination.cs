using Microsoft.EntityFrameworkCore;

namespace WebBlog.API.Models.Pagination
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; } = [];
        public int Index { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages { get; private set; }


        public Pagination(List<T> items, int index, int size, int totalItems)
        {
            Items = items;
            Index = index;
            Size = size;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((double)totalItems / size);
        }
        public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int index, int size)
        {
            var totalItems = await source.CountAsync();
            var items = await source.Skip((index - 1) * size).Take(size).ToListAsync();
            return new Pagination<T>(items, index, size, totalItems);
        }
    }
}
