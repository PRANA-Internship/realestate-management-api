namespace RS.Infrastructure.Configurations;

public class ChapaSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.chapa.co/v1/";
    public string CallbackUrl { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
}
