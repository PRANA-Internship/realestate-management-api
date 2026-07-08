using System.Text.Json.Serialization;

namespace RS.Application.Common.Models;

public class ChapaInitializeRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "ETB";

    [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = default!;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = default!;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = default!;

    [JsonPropertyName("tx_ref")]
    public string TxRef { get; set; } = default!;

    [JsonPropertyName("callback_url")]
    public string? CallbackUrl { get; set; }

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; set; }

    [JsonPropertyName("customization")]
    public ChapaCustomization? Customization { get; set; }
}

public class ChapaCustomization
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class ChapaInitializeResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!; // "success" or "failed"

    [JsonPropertyName("data")]
    public ChapaInitializeData? Data { get; set; }
}

public class ChapaInitializeData
{
    [JsonPropertyName("checkout_url")]
    public string CheckoutUrl { get; set; } = default!;
}

public class ChapaVerifyResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = default!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!; // "success" or "failed"

    [JsonPropertyName("data")]
    public ChapaVerifyData? Data { get; set; }
}

public class ChapaVerifyData
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!; // "success", "failed", "pending"

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;

    [JsonPropertyName("tx_ref")]
    public string TxRef { get; set; } = default!;

    [JsonPropertyName("reference")]
    public string? Reference { get; set; }
}
