using AutoMapper;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.WebApi.DTO;
using PersonalBlog.WebApi.utils.ApiResult;
using SqlSugar;

namespace PersonalBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _blogNewsService;
        public BlogNewsController(IBlogNewsService blogNewsService)
        {
            _blogNewsService = blogNewsService;
        }


        [HttpGet("GetBlogNews")]
        public async Task<ActionResult<ApiResult>> GetBlogNews()
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var result = await _blogNewsService.QueryAsync(c => c.AuthorId == id);
            if (result == null) return ApiResultHelper.Failed("未找到任何博客新闻");
            return ApiResultHelper.Success(result, result.Count);
        }

        [HttpGet("GetBlogNewsByPage")]
        public async Task<ActionResult<ApiResult>> GetBlogNewsByPage([FromServices]IMapper iMapper,int page, int size)
        {
            RefAsync<int> total = 0;
            var blogNews = await _blogNewsService.QueryAsync(page, size, total);
            try
            {
                var blogNewsDTO = iMapper.Map<List<BlogNewsDTO>>(blogNews);
                return ApiResultHelper.Success(blogNewsDTO, total);
            }
            catch (Exception)
            {
                return ApiResultHelper.Failed("获取文章失败，服务器发生错误");
            }
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet("CreateNews")]
        public async Task<ActionResult<ApiResult>> Create(string title, string content, int typeId)
        {
            BlogNews blogNews = new BlogNews
            {
                PageViews = 0,
                Title = title,
                Content = content,
                Time = DateTime.Now,
                LikesNums = 0,
                TypeId = typeId,
                AuthorId = Convert.ToInt32(this.User.FindFirst("Id").Value)
            };

            bool res = await _blogNewsService.CreateAsync(blogNews);
            if (res)
            {
                return ApiResultHelper.Success(blogNews);
            }
            return ApiResultHelper.Failed("添加文章失败，服务器发生错误");
        }

        [HttpDelete("DeleteBlogs")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            bool res = await _blogNewsService.DeleteAsync(id);
            if (res)
            {
                return ApiResultHelper.Success(res);
            }
            return ApiResultHelper.Failed("删除文章失败，服务器发生错误");
        }

        [HttpPut("EditBlogs")]
        public async Task<ActionResult<ApiResult>> Update(int id, string title, string content, int typeId)
        {
            var blogNews = await _blogNewsService.FindAsync(id);
            if (blogNews == null)
            {
                return ApiResultHelper.Failed("没有找到改文章");
            }
            blogNews.Title = title;
            blogNews.Content = content;
            blogNews.TypeId = typeId;
            bool res = await _blogNewsService.EditAsync(blogNews);
            if (res)
            {
                return ApiResultHelper.Success(res);
            }
            return ApiResultHelper.Failed("更新文章失败，服务器发生错误");
        }
    }
}
