using AutoMapper;
using Blog.Entity;
using Blog.Shared;

namespace Blog.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateArticleRequest, Article>()
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore());

        CreateMap<CreateCategoryRequest, Category>()
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore());

        CreateMap<UpdateArticleRequest, Article>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<UpdateCategoryRequest, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
