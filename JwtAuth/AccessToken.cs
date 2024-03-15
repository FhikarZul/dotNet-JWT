using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtAuth;

public class AccessToken
{
    static public readonly string KEY_USER_ID = "UserID";

    private readonly AuthConfiguration _options;

    public AccessToken(IOptions<AuthConfiguration> options)
    {
        _options = options.Value;
    }

    public string Generate(string id, int expireTimeInDays)
    {
        List<Claim> claims = new() {
                new Claim(KEY_USER_ID, id),
            };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey!));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(expireTimeInDays),
                signingCredentials: cred
            );

        var result = new JwtSecurityTokenHandler().WriteToken(token);


        return result;
    }
}

