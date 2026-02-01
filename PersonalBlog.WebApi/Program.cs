using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Repository;
using Blog.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalBlog.WebApi.AutoMapper;
using SqlSugar;
using SqlSugar.IOC;

namespace PersonalBlog.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 安装包：SqlSugar.IOC + SqlSugar（或 SqlSugarCore）
            builder.Services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = builder.Configuration["SqlConn"],
                DbType = IocDbType.SqlServer,
                IsAutoCloseConnection = true
            });

            // 可选：全局AOP配置（强烈建议加）
            SugarIocServices.ConfigurationSugar(db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(UtilMethods.GetNativeSql(sql, pars));
                };
            });

            #region IOC 依赖注入
            builder.Services.AddCustomIOC();
            #endregion

            #region  JWT鉴权
            builder.Services.AddCustomJWT();
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(CustomAutoMapperProfile));
            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // 1️⃣ 定义 Bearer 鉴权方案
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT 授权，请在此处输入：Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // 2️⃣ 全局启用鉴权
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            #region Swagger使用鉴权

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //鉴权
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            #region 异常处理
            app.UseExceptionHandler(builder=> { 
                builder.Run(async context=> {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync("内部错误");
                });
            });
            #endregion
            app.MapControllers();

            app.Run();
        }
    }

    public static class IOCExtend
    {
        public static IServiceCollection AddCustomIOC(this IServiceCollection services)
        {
            services.AddScoped<IBlogNewsRepository, BlogNewsRepository>();
            services.AddScoped<IBlogNewsService, BlogNewsService>();
            services.AddScoped<ITypeInfoRepository, TypeInfoRepository>();
            services.AddScoped<ITypeInfoService, TypeInfoService>();
            services.AddScoped<IWriterInfoRepository, WriterInfoRepository>();
            services.AddScoped<IWriterInfoService, WriterInfoService>();

            return services;
        }

        public static IServiceCollection AddCustomJWT(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:6060",
                        ValidAudience = "http://localhost:5000",
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSSFA-SADHJVF-VF")),
                        ClockSkew = TimeSpan.FromMinutes(60)
                    };
                });
            return services;
        }
    }
}