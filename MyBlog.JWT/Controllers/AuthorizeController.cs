using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.JWT.utils.ApiResult;
using MyBlog.JWT.utils.MD5;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;
        public AuthorizeController(IWriterInfoService writerInfoService)
        {
            _writerInfoService = writerInfoService;
        }

        [HttpPost("Login")]
        public async Task<ApiResult> Login(string name, string userpsw)
        {
            string pwd = MD5Helper.MD5Encrypt32(userpsw);
            //数据校验
            var writer = await _writerInfoService.FindAsync(c => c.Name == name && c.UserPwd == pwd);
            if (writer != null)
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,writer.Name),
                    new Claim("Id",writer.Id.ToString()),
                    new Claim("UserName",writer.UserName),
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSSFA-SADHJVF-VF"));

                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Success(jwtToken);
            }
            else
            {
                return ApiResultHelper.Failed("用户名或密码错误");
            }
        }
    }
}
