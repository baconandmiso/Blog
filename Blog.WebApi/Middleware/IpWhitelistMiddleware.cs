namespace Blog.WebApi.Endpoints;

public class IpWhitelistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpWhitelistMiddleware> _logger;
    private readonly List<string> _whitelist;

    public IpWhitelistMiddleware(RequestDelegate next, ILogger<IpWhitelistMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        // appsettings.jsonから "AdminIpWhitelist" セクションを読み込む
        _whitelist = configuration.GetSection("AdminIpWhitelist").Get<List<string>>() ?? new List<string>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 管理者向けAPIのエンドポイントかを判定する
        // ここではURLが "/api/auth" または "/api/articles" で始まるものを対象とする
        var path = context.Request.Path.Value ?? "";
        if (!path.StartsWith("/api/auth") && !path.StartsWith("/api/articles") && !path.StartsWith("/api/categories") && !path.StartsWith("/api/profile"))
        {
            // 対象外のエンドポイントなら、何もせず次の処理へ
            await _next(context);
            return;
        }
        else
        {
            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // GETリクエストはIP制限の対象外とする
                await _next(context);
                return;
            }
        }

        // リクエスト元のIPアドレスを取得する
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

        _logger.LogInformation("Request from Remote IP address: {RemoteIp}", remoteIp);

        // IPアドレスがホワイトリストに含まれているかチェック
        if (remoteIp is null || !_whitelist.Contains(remoteIp))
        {
            _logger.LogWarning("Forbidden request from IP address: {RemoteIp}", remoteIp);
            // 許可されていないIPアドレスからのアクセスはここで処理を中断し、403 Forbiddenを返す
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden: Access is restricted.");
            return;
        }

        // 許可されたIPアドレスなら、次のミドルウェア（コントローラなど）へ処理を渡す
        await _next(context);
    }
}