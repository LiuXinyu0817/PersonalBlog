using AutoMapper;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.WebApi.DTO;
using PersonalBlog.WebApi.utils.ApiResult;
using PersonalBlog.WebApi.utils.MD5;

namespace PersonalBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WriterController : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;
        public WriterController(IWriterInfoService writerInfoService)
        {
            _writerInfoService = writerInfoService;
        }

        [HttpPost("AddWriter")]
        public async Task<ApiResult> AddWriter(string writerName, string userName, string psw)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(writerName))
            {
                return ApiResultHelper.Failed("作者名称不能为空");
            }
            #endregion
            WriterInfo writerInfo = new WriterInfo
            {
                Name = writerName,
                UserName = userName,
                //加密密码
                UserPwd = MD5Helper.MD5Encrypt32(psw)
            };
            var existWriter = await _writerInfoService.FindAsync(c => c.Name == writerName);
            if (existWriter != null)
            {
                return ApiResultHelper.Failed("账号已存在");
            }
            bool res = await _writerInfoService.CreateAsync(writerInfo);
            if (res)
            {
                return ApiResultHelper.Success(writerInfo);
            }
            return ApiResultHelper.Failed("添加失败");
        }

        [HttpPut("Edit")]
        public async Task<ApiResult> Edit(string Name)
        {
            int id = Convert.ToInt32(User.FindFirst("Id")?.Value);
            var writer = await _writerInfoService.FindAsync(id);
            writer.Name = Name;
            bool res = await _writerInfoService.EditAsync(writer);
            if (res)
            {
                return ApiResultHelper.Success("修改成功");
            }
            else
                return ApiResultHelper.Failed("修改失败");
        }

        [AllowAnonymous]
        [HttpGet("GetWriterInfo")]
        public async Task<ApiResult> GetWriterInfo([FromServices] IMapper mapper, int id)
        {
            var writer = await _writerInfoService.FindAsync(id);
            if (writer != null)
            {
                var writerDTO = mapper.Map<WriterDTO>(writer);
                return ApiResultHelper.Success(writerDTO);
            }
            else
                return ApiResultHelper.Failed("未找到作者信息");
        }
    }
}