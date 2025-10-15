using Blog.Repository;
using Blog.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Blog.WebApi.Endpoints;
using Blog.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        var jwtKey = builder.Configuration.GetSection("Jwt").GetValue<string>("Key");

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT key is missing in configuration.");
        }

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
            };
    });

builder.Services.AddAuthorization();

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
            options.UseNpgsql(builder.Configuration.GetConnectionString("blogdb"));
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
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);

builder.Services.AddSingleton<IMapper, ServiceMapper>();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseMiddleware<IpWhitelistMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.RegisterArticlesEndpoints();
app.RegisterCategoriesEndpoints();
app.RegisterUserLoginEndpoints();
app.RegisterFileEndpoints();

var storagePath = app.Configuration.GetValue<string>("FileStoragePath")
                  ?? Path.Combine(Directory.GetCurrentDirectory(), "Storage");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "thumbnails")),
    RequestPath = "/thumbnails"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "articles", "attachments")),
    RequestPath = "/articles/attachments"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "profile", "icon")),
    RequestPath = "/profile/icon"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "profile", "banner")),
    RequestPath = "/profile/banner"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "emojies")),
    RequestPath = "/emojies"
});

app.Run();