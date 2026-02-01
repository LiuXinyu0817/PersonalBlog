using Blog.Model;
using SqlSugar;

namespace PersonalBlog.WebApi.DTO
{
    public class BlogNewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public int LikesNums { get; set; }
        public int TypeId { get; set; }
        public int AuthorId { get; set; }
        public string TypeName { get; set; }
        public string AuthorName { get; set; }
    }
}
