using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using isc.time.report.be.domain.Entity.Auth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace isc.time.report.be.application.Utils.Auth
{
    public class JWTUtils
    {

        private readonly IConfiguration configuration;

        public JWTUtils(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:JWTSecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("id", user.Id.ToString())
    };

            // Añadir roles directamente
            if (user.UsersRols != null)
            {
                foreach (var ur in user.UsersRols)
                {
                    claims.Add(new Claim(ClaimTypes.Role, ur.Rols.RolName));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
