using AutoMapper;
using Blog.Model;
using PersonalBlog.WebApi.DTO;
using SQLitePCL;

namespace PersonalBlog.WebApi.AutoMapper
{
    public class CustomAutoMapperProfile : Profile
    {
        public CustomAutoMapperProfile()
        {
            base.CreateMap<WriterInfo, WriterDTO>();
            base.CreateMap<BlogNews, BlogNewsDTO>()
                .ForMember(dest => dest.TypeName, source => source.MapFrom(src => src.TypeInfo.TypeName))
                .ForMember(dest => dest.AuthorName, source => source.MapFrom(src => src.WriterInfo.Name));
        }
    }
}
