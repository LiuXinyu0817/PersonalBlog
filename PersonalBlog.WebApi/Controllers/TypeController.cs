using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.WebApi.utils.ApiResult;
using System.Reflection;
using TypeInfo = Blog.Model.TypeInfo;

namespace PersonalBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly ITypeInfoService _typeInfoService;
        public TypeController(ITypeInfoService typeInfoService)
        {
            _typeInfoService = typeInfoService;
        }


        [HttpGet("GetTypeInfo")]
        public async Task<ApiResult> GetTypeInfo()
        {
            var tyeps = await _typeInfoService.QueryAsync();
            if (tyeps.Count == 0)
            {
                return ApiResultHelper.Failed();
            }
            return ApiResultHelper.Success(tyeps);
        }

        [HttpPost("AddType")]
        public async Task<ApiResult> AddType(string typeName)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return ApiResultHelper.Failed("类型名称不能为空");
            }
            #endregion
            TypeInfo typeInfo = new TypeInfo
            {
                TypeName = typeName
            };
            bool res = await _typeInfoService.CreateAsync(typeInfo);
            if (res)
            {
                return ApiResultHelper.Success(res);
            }
            return ApiResultHelper.Failed("添加失败");
        }

        [HttpPut("UpdateType")]
        public async Task<ApiResult> UpdateType(int id, string typeName)
        {
            #region 数据验证
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return ApiResultHelper.Failed("类型名称不能为空");
            }
            #endregion
            var type = await _typeInfoService.FindAsync(id);
            if (type == null)
            {
                return ApiResultHelper.Failed("没有找到该文章类型");
            }
            type.TypeName = typeName;
            bool res = await _typeInfoService.EditAsync(type);
            if (res)
            {
                return ApiResultHelper.Success(res);
            }
            return ApiResultHelper.Failed("没有找到该文章类型");
        }

        [HttpDelete("DeleteType")]
        public async Task<ApiResult> DeleteType(int id)
        {
            bool res = await _typeInfoService.DeleteAsync(id);
            if (res)
            {
                return ApiResultHelper.Success(res);
            }
            return ApiResultHelper.Failed("删除失败");
        }
    }
}
