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
using System.Text.Json;

namespace isc.time.report.be.application.Utils.Auth
{
    public class JWTUtils
    {

        private readonly IConfiguration configuration;

        public JWTUtils(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user, List<string> modulePaths, int expiryMinutes = 60, bool isRecovery = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:JWTSecretKey"])
            );

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var normalizedModules = (modulePaths ?? new List<string>())
                .Select(m => m.ToLower())
                .Distinct()
                .ToList();

            var roleId = user.UserRole?
                .FirstOrDefault(r => r.Status)?.RoleID;

            if (roleId == null)
                throw new Exception("Usuario sin rol asignado");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserID", user.Id.ToString()),
        new Claim("EmployeeID", user.EmployeeID.ToString()),
        new Claim("PersonID", user.Employee?.PersonID.ToString() ?? "0"),
        new Claim("roleId", roleId.Value.ToString()),
        new Claim("modules", JsonSerializer.Serialize(normalizedModules))
    };

            if (user.UserRole != null)
            {
                foreach (var ur in user.UserRole.Where(ur => ur.Status))
                {
                    if (ur.Role != null && !string.IsNullOrEmpty(ur.Role.RoleName))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, ur.Role.RoleName));
                    }
                }
            }

            if (isRecovery)
            {
                claims.Add(new Claim("recover-password", "true"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateTokenAndGetPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JWT:JWTSecretKey"]);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
