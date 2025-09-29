using Blog.Entity;
using Blog.Shared;
using Mapster;

namespace Blog.Services;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // CreateArticleRequest から Article へのマッピング
        config.NewConfig<CreateArticleRequest, Article>()
            .Ignore(dest => dest.ArticleCategories);

        // CreateCategoryRequest から Category へのマッピング
        config.NewConfig<CreateCategoryRequest, Category>()
            .IgnoreNullValues(true);

        // UpdateArticleRequest から Article へのマッピング
        // Mapsterでは、デフォルトでnullのプロパティは無視されます。
        // 明示的に設定する場合は以下のようにします。
        config.NewConfig<UpdateArticleRequest, Article>()
            .IgnoreNullValues(true)
            .Ignore(dest => dest.ArticleCategories);

        // UpdateCategoryRequest から Category へのマッピング
        config.NewConfig<UpdateCategoryRequest, Category>()
            .IgnoreNullValues(true);

        // Category から CategoryResponse へのマッピング
        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

        // Article から ArticleResponse へのマッピング
        config.NewConfig<Article, ArticleResponse>()
            .Map(dest => dest.Categories, src => src.ArticleCategories.Select(ac　=> ac.Category));
    }
}
