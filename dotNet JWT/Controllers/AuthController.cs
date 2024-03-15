using JwtAuth;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotNet_JWT.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AccessToken _accessToken;

        public AuthController(AccessToken accessToken)
        {
            _accessToken = accessToken;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            if (username == "fhikar" && password == "123")
            {
                string token = _accessToken.Generate("user_id_123", expireTimeInDays: 7);

                TokenHttpCookies.Set(httpContext: HttpContext, accessToken: token);

                return Ok(new { accessToken = token });
            }

            return BadRequest();
        }
    }
    
}

