using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectApi.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectApi.Services
{
    public class JwtService
    {
        private readonly JwtOptions jwtOptions;

        public JwtService(IOptions<JwtOptions> options)
        {
            this.jwtOptions = options.Value;
        }

        public string GenerateToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@test.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.DurationInMinutes),
                signingCredentials: credintials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
