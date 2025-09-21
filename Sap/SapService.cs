using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConteiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class SapService
{
    private readonly SapConexao _sapManager;
    private readonly SAPSettingsModel _settings;

    public SapService(SapConexao sapManager, IOptions<SAPSettingsModel> settings)
    {
        _sapManager = sapManager;
        _settings = settings.Value;
    }

    public async Task<IActionResult> SendRequestAsync(
        HttpMethod method,
        string endpoint,
        object payload = null,
        Dictionary<string, string> queryParams = null,
        bool handlePagination = false)
    {
        var allData = new List<JsonElement>();
        var baseUrl = _settings.Url.TrimEnd('/');
        var currentUrl = $"{baseUrl}/{endpoint}";
        HttpResponseMessage response = null;

        try
        {
            var client = await _sapManager.GetSessionAsync();

            bool isFirstRequest = true;

            while (true)
            {
                var requestUrl = isFirstRequest
                    ? AppendQueryParams(currentUrl, queryParams)
                    : currentUrl; // Já vem completo do odata.nextLink

                var request = new HttpRequestMessage(method, requestUrl);

                if (payload != null && method != HttpMethod.Get)
                {
                    var json = JsonSerializer.Serialize(payload);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                response = await client.SendAsync(request);
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                var content = Encoding.UTF8.GetString(responseBytes);

                if (response.IsSuccessStatusCode)
                {
                    if (method == HttpMethod.Get && handlePagination)
                    {
                        using var doc = JsonDocument.Parse(content);
                        var root = doc.RootElement;

                        if (root.TryGetProperty("value", out var value) && value.ValueKind == JsonValueKind.Array)
                        {
                            var rawJson = value.GetRawText();
                            var parsedItems = JsonSerializer.Deserialize<List<JsonElement>>(rawJson);
                            allData.AddRange(parsedItems);
                        }

                        if (root.TryGetProperty("odata.nextLink", out var nextLink))
                        {
                            currentUrl = nextLink.GetString().StartsWith("http")
                                ? nextLink.GetString()
                                : $"{baseUrl}/{nextLink.GetString()}";

                            isFirstRequest = false;
                            continue;
                        }

                        return new OkObjectResult(new { value = allData });
                    }

                    return new OkObjectResult(JsonSerializer.Deserialize<object>(content));
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                        (response.StatusCode == System.Net.HttpStatusCode.BadRequest && content.Contains("Session")))
                    {
                        _sapManager.InvalidateSession();
                        return new UnauthorizedObjectResult("Sessão SAP expirada ou inválida. Tente novamente.");
                    }

                    return new ObjectResult($"Erro SAP ({(int)response.StatusCode}): {content}")
                    {
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
        }
        catch (HttpRequestException ex)
        {
            return new ObjectResult($"Erro de conexão com a Service Layer SAP: {ex.Message}")
            {
                StatusCode = 503
            };
        }
        catch (Exception ex)
        {
            var statusCode = response?.StatusCode != null ? (int)response.StatusCode : 500;
            return new ObjectResult($"Erro interno inesperado: {ex.Message}")
            {
                StatusCode = statusCode
            };
        }
    }

    private string AppendQueryParams(string url, Dictionary<string, string> queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
            return url;

        var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{url}?{query}";
    }
}
