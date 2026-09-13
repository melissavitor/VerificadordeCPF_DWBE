using System.Text.Json;

namespace VerificadordeCPF.Services;

public class CpfBrasilApiClient {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CpfBrasilApiClient(HttpClient httpClient, IConfiguration configuration) {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<CpfBrasilResultado> ConsultarAsync(string cpfLimpo) {
        var apiKey = _configuration["CpfBrasilApi:ApiKey"];

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.cpf-brasil.org/cpf/{cpfLimpo}");
        request.Headers.Add("X-API-Key", apiKey);

        using var response = await _httpClient.SendAsync(request);
        var conteudo = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) {
            return new CpfBrasilResultado { Sucesso = false, Erro = $"Status {(int)response.StatusCode}" };
        }

        using var doc = JsonDocument.Parse(conteudo);
        var sucesso = doc.RootElement.TryGetProperty("success", out var successEl)
                      && successEl.ValueKind == JsonValueKind.True;

        return new CpfBrasilResultado { Sucesso = sucesso };
    }
}

public class CpfBrasilResultado {
    public bool Sucesso { get; set; }
    public string? Erro { get; set; }
}