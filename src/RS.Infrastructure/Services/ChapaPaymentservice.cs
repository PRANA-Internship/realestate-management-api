// RS.Infrastructure/Services/ChapaPaymentService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RS.Application.Common.Interfaces;
using RS.Contracts.Payments;

namespace RS.Infrastructure.Services;

public class ChapaPaymentService(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<ChapaPaymentService> logger) : IPaymentService
{
    public async Task<InitializePaymentResponse> InitializePaymentAsync(
        InitializePaymentRequest request, CancellationToken ct = default)
    {
        var secretKey = configuration["ChapaSettings:SecretKey"];
        var callbackUrl = configuration["ChapaSettings:CallbackUrl"];
        var returnUrl = configuration["ChapaSettings:ReturnUrl"];

        var txRef = $"rs-{Guid.NewGuid():N}";

        var payload = new
        {
            amount = request.Amount.ToString("F2"),
            currency = request.Currency,
            email = request.Email,
            first_name = request.FirstName,
            last_name = request.LastName,
            phone_number = request.PhoneNumber,
            tx_ref = txRef,
            callback_url = callbackUrl,
            return_url = returnUrl
        };

        logger.LogWarning("Chapa payload being sent: {Payload}", JsonSerializer.Serialize(payload));

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        var response = await httpClient.PostAsJsonAsync("transaction/initialize", payload, ct);
        var responseBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Chapa initialize failed with {StatusCode}: {Body}", response.StatusCode, responseBody);
            throw new InvalidOperationException($"Chapa initialize failed: {responseBody}");
        }

        var result = JsonSerializer.Deserialize<ChapaInitializeApiResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (result?.Data?.CheckoutUrl is null)
        {
            logger.LogError("Chapa initialize returned no checkout_url: {Body}", responseBody);
            throw new InvalidOperationException("Failed to initialize Chapa payment.");
        }

        return new InitializePaymentResponse(result.Data.CheckoutUrl, txRef);
    }

    public async Task<bool> VerifyPaymentAsync(string txRef, CancellationToken ct = default)
    {
        var secretKey = configuration["ChapaSettings:SecretKey"];

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        var response = await httpClient.GetAsync($"transaction/verify/{txRef}", ct);
        var responseBody = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Chapa verify failed with {StatusCode}: {Body}", response.StatusCode, responseBody);
            return false;
        }

        var result = JsonSerializer.Deserialize<ChapaVerifyApiResponse>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Data?.Status == "success";
    }

    private class ChapaInitializeApiResponse
    {
        [JsonPropertyName("data")]
        public ChapaInitializeData? Data { get; set; }
    }

    private class ChapaInitializeData
    {
        [JsonPropertyName("checkout_url")]
        public string? CheckoutUrl { get; set; }
    }

    private class ChapaVerifyApiResponse
    {
        [JsonPropertyName("data")]
        public ChapaVerifyData? Data { get; set; }
    }

    private class ChapaVerifyData
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
