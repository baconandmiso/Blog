using AutoMapper;
using Blog.Entity;
using Blog.Shared;

namespace Blog.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateArticleRequest, Article>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
