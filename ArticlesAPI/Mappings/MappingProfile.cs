using ArticlesAPI.DTOs.Command;
using ArticlesAPI.Models;
using AutoMapper;

namespace ArticlesAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateArticleCommand, Article>();
            CreateMap<UpdateArticleCommand, Article>();
        }
    }
}
