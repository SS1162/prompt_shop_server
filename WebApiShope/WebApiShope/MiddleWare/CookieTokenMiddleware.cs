namespace WebApiShope.MiddleWare
{
    public class CookieTokenMiddleware(RequestDelegate next)
    {
        public Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization")
                && context.Request.Cookies.TryGetValue("access_token", out var token))
            {
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }

            return next(context);
        }
    }

    public static class CookieTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieToken(this IApplicationBuilder app)
            => app.UseMiddleware<CookieTokenMiddleware>();
    }
}
