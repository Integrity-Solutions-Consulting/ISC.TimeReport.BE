using isc.time.report.be.domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Utils.Peticiones
{
    public class HttpUtils
    {
        public async Task<T?> SendRequest<T>(string url, HttpMethod method, object? body = null, string? token = null)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(method, url);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                if (body != null && (method == HttpMethod.Post || method == HttpMethod.Put))
                {
                    request.Content = JsonContent.Create(body);
                }

                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"[HTTP ERROR] URL: {url}");
                    Console.WriteLine($"StatusCode: {response.StatusCode}");
                    Console.WriteLine($"Response Body: {errorContent}");

                    throw new ServerFaultException(
                        $"La solicitud HTTP falló. Código: {(int)response.StatusCode} - {response.StatusCode}. Contenido: {errorContent}"
                    );
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }
        }
    }
}
