using Microsoft.AspNetCore.Http;

namespace JwtAuth
{
    public class TokenHttpCookies
    {
        private static readonly string COOKIES_KEY = "access_token";

        public static void Set(HttpContext httpContext, string accessToken, int? expireTimeInDays = null)
        {
            var option = new CookieOptions();

            if (expireTimeInDays.HasValue)
                option.Expires = DateTimeOffset.Now.AddDays(expireTimeInDays.Value);
            else
                option.Expires = DateTimeOffset.Now.AddDays(1);

            option.Domain = httpContext.Request.Host.Host;
            option.Path = "/";
            option.HttpOnly = true;

            httpContext.Response.Cookies.Append(COOKIES_KEY, accessToken, option);

            Console.WriteLine("Access Token is saved to cookie");
        }

        public static string? Get(HttpContext httpContext)
        {
            return httpContext.Request.Cookies[COOKIES_KEY];
        }

    }
}

