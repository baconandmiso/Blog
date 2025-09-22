using Blog.Repository;
using Blog.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApi.Endpoints;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}

builder.AddServiceDefaults();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var provider = builder.Configuration.GetValue<string>("DatabaseProvider");

    switch (provider)
    {
        case "MySQL":
            options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection") ?? "");
            break;
        case "PostgreSQL":
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
            break;
        case "SQLite":
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
            break;
        default:
            throw new Exception($"Unsupported database provider: {provider}");
    }
});

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(x => x.AddProfile<MappingProfile>());

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStaticFiles();

app.UseHttpsRedirection();

// TODO: 許可されたIPアドレスのみ管理画面, ログインAPIを通せるように設定する。(CFを経由してアクセスすることも考慮すること。)

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.RegisterArticlesEndpoints();
app.RegisterCategoriesEndpoints();
app.RegisterUserLoginEndpoints();

app.Run();