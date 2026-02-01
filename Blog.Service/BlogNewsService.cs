using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class BlogNewsService : BaseService<BlogNews>, IBlogNewsService
    {
        private readonly IBlogNewsRepository _blogNewsRepository;
        public BlogNewsService(IBlogNewsRepository blogNewsRepository)
        {
            base._IBaseRepository = blogNewsRepository;
            _blogNewsRepository = blogNewsRepository;
        }
    }
}
