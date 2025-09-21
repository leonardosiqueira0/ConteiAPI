using ConteiAPI.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class SapConexao
{
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private static SapConexao _instance;

    private readonly SAPSettingsModel _settings;
    private HttpClient _client;
    private DateTime _lastLoginTime;
    private readonly TimeSpan _sessionExpiryThreshold = TimeSpan.FromMinutes(25);

    private SapConexao(SAPSettingsModel settings)
    {
        _settings = settings;
        _client = new HttpClient();
    }

    public static SapConexao GetInstance(SAPSettingsModel settings)
    {
        if (_instance == null)
        {
            _instance = new SapConexao(settings);
        }
        return _instance;
    }

    public async Task<HttpClient> GetSessionAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (IsSessionExpired())
            {
                Console.WriteLine("Sessão expirada. Realizando novo login...");
                await PerformLoginAsync();
            }
            return _client;
        }
        finally
        {
            _lock.Release();
        }
    }

    private bool IsSessionExpired()
    {
        return _client == null || DateTime.Now - _lastLoginTime > _sessionExpiryThreshold;
    }

    private async Task PerformLoginAsync()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _client = new HttpClient(handler);

        var loginPayload = new
        {
            CompanyDB = _settings.Db,
            UserName = _settings.Usernamesap,
            Password = _settings.Password
        };

        var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"{_settings.Url}/Login", content);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Erro ao logar no SAP: {response.StatusCode} - {error}");
            throw new Exception($"Erro ao logar no SAP: {response.StatusCode} - {error}");
        }

        _lastLoginTime = DateTime.Now;
        Console.WriteLine("Login SAP realizado com sucesso.");
    }

    public void InvalidateSession()
    {
        _client = new HttpClient();
        _lastLoginTime = DateTime.MinValue;
        Console.WriteLine("Sessão SAP invalidada.");
    }
}

