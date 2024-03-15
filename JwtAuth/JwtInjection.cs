using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;

namespace JwtAuth
{
	public static class JwtInjection
	{
        readonly static string OBJ_AUTH_SECRET_KEY = "AuthSettings:SecretKey";
        readonly static string OBJ_AUTH_SETTING = "AuthSettings";

        public static void JwtConfigureServices(this IServiceCollection services, WebApplicationBuilder? builder)
        {
            services.Configure<AuthConfiguration>(builder!.Configuration.GetSection(OBJ_AUTH_SETTING));
            services.AddSingleton<AccessToken>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option => {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            builder.Configuration.GetSection(OBJ_AUTH_SECRET_KEY).Value!)
                        ),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                }
             );

            services.Configure<AuthConfiguration>(builder.Configuration.GetSection(OBJ_AUTH_SETTING));
            services.AddSingleton<AccessToken>();

        }

        public static void JwtConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<JwtCookieMiddleware>();
        }
    }
}

