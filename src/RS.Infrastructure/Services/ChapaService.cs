using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RS.Application.Common.Interfaces;
using RS.Application.Common.Models;
using RS.Infrastructure.Configurations;

namespace RS.Infrastructure.Services;

public class ChapaService : IChapaService
{
    private readonly HttpClient _httpClient;
    private readonly ChapaSettings _settings;

    public ChapaService(HttpClient httpClient, IOptions<ChapaSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.SecretKey);
    }

    public async Task<ChapaInitializeResponse> InitializePaymentAsync(ChapaInitializeRequest request, CancellationToken ct = default)
    {
        request.CallbackUrl ??= _settings.CallbackUrl;
        request.ReturnUrl ??= _settings.ReturnUrl;

        if (string.IsNullOrWhiteSpace(_settings.SecretKey) || _settings.SecretKey.Contains("xxxxxxxx"))
        {
            return new ChapaInitializeResponse
            {
                Status = "success",
                Message = "Mocked payment initialization success.",
                Data = new ChapaInitializeData
                {
                    CheckoutUrl = $"https://checkout.chapa.co/checkout/payment/mock-{request.TxRef}"
                }
            };
        }

        var response = await _httpClient.PostAsJsonAsync("transaction/initialize", request, ct);

        if (response.IsSuccessStatusCode)
        {
            var successResult = await response.Content.ReadFromJsonAsync<ChapaInitializeResponse>(cancellationToken: ct);
            return successResult ?? new ChapaInitializeResponse { Status = "failed", Message = "Failed to deserialize response." };
        }

        var errorContent = await response.Content.ReadAsStringAsync(ct);
        return new ChapaInitializeResponse
        {
            Status = "failed",
            Message = $"Chapa payment initialization failed with status: {response.StatusCode}. Content: {errorContent}"
        };
    }

    public async Task<ChapaVerifyResponse> VerifyTransactionAsync(string txRef, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.SecretKey) || _settings.SecretKey.Contains("xxxxxxxx"))
        {
            return new ChapaVerifyResponse
            {
                Status = "success",
                Message = "Mocked payment verification success.",
                Data = new ChapaVerifyData
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = "success",
                    Amount = 100,
                    Currency = "ETB",
                    TxRef = txRef,
                    Reference = $"MOCK_REF_{Guid.NewGuid().ToString().Substring(0, 8)}"
                }
            };
        }

        var response = await _httpClient.GetAsync($"transaction/verify/{txRef}", ct);

        if (response.IsSuccessStatusCode)
        {
            var successResult = await response.Content.ReadFromJsonAsync<ChapaVerifyResponse>(cancellationToken: ct);
            return successResult ?? new ChapaVerifyResponse { Status = "failed", Message = "Failed to deserialize response." };
        }

        var errorContent = await response.Content.ReadAsStringAsync(ct);
        return new ChapaVerifyResponse
        {
            Status = "failed",
            Message = $"Chapa payment verification failed with status: {response.StatusCode}. Content: {errorContent}"
        };
    }
}
