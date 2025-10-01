using Blog.Repository;
using Blog.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApi.Endpoints;
using WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // トークンの検証ルールを設定
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

builder.Services.AddSingleton<IMapper, ServiceMapper>();

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