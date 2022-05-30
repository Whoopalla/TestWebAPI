using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebAPI.Core.Interfaces;

namespace TestWebAPI.Infrastructure {
    public class TokenService : ITokenService {

        public TokenService() {
        }

        public async Task<string> GetTokenAsync(string login) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("792ch9mc&@92VW4QTVQT93V039jcw0[T43"); //Environment.GetEnvironmentVariable("secretKey")
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
