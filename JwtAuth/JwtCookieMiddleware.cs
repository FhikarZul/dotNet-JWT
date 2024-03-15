using Microsoft.AspNetCore.Http;

namespace JwtAuth;
public class JwtCookieMiddleware
{
    private readonly RequestDelegate _next;

    public JwtCookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = TokenHttpCookies.Get(context); 

        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers["Authorization"] = "Bearer " + token;
        } else
        {
            context.Request.Headers["Authorization"] = "Bearer " + context.Request.Headers["Authorization"];
        }

        await _next(context);
    }
}