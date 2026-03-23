using isc.time.report.be.domain.Entity.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            var activeRoles = user.UserRole?.Where(r => r.Status).ToList();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserID", user.Id.ToString()),
                new Claim("EmployeeID", user.EmployeeID.ToString()),
                new Claim("PersonID", user.Employee?.PersonID.ToString() ?? "0"),
                // En recuperación no es necesario asegurar roles; el endpoint solo valida `recover-password` y `UserID`.
                new Claim("modules", JsonSerializer.Serialize(normalizedModules))
            };

            // En tokens de recuperación permitimos que el usuario no tenga roles cargados/activos.
            if (activeRoles != null && activeRoles.Any())
            {
                var primaryRoleId = activeRoles.First().RoleID;
                var allRoleIds = activeRoles.Select(r => r.RoleID.ToString()).ToList();

                claims.Add(new Claim("RoleID", primaryRoleId.ToString()));
                claims.Add(new Claim("RoleIDs", string.Join(",", allRoleIds)));

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
            }
            else if (!isRecovery)
            {
                throw new Exception("Usuario sin rol asignado");
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
