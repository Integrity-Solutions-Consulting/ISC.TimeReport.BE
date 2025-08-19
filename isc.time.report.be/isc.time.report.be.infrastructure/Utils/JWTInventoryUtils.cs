using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Utils
{
    public class JWTInventoryUtils
    {
        public DateTime GetExpirationDateFromToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
                throw new ArgumentException("El token no es un JWT válido.");

            var payload = parts[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var payloadJson = Encoding.UTF8.GetString(jsonBytes);

            var payloadData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadJson);

            if (!payloadData.TryGetValue("exp", out var expValue))
                throw new InvalidOperationException("El token no contiene el campo 'exp'.");

            var expSeconds = expValue.GetInt64();
            var expiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

            return expiration;
        }

        private string PadBase64(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}
