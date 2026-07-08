using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RS.Application.Common.Interfaces;

namespace RS.Infrastructure.Services;

public class SupabaseStorageService : IStorageService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SupabaseStorageService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> UploadImageAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var projectUrl = _configuration["Supabase:Url"]!;
        var bucket = _configuration["Supabase:Bucket"]!;
        var secretKey = _configuration["Supabase:SecretKey"]!;

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        if (string.IsNullOrWhiteSpace(projectUrl) || projectUrl.Contains("projectId.supabase.co", StringComparison.OrdinalIgnoreCase))
        {
            return $"https://mockstorage.com/storage/v1/object/public/{bucket}/{fileName}";
        }

        var uploadUrl =
            $"{projectUrl}/storage/v1/object/{bucket}/{fileName}";

        using var stream = file.OpenReadStream();

        using var content = new StreamContent(stream);

        content.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        using var request =
            new HttpRequestMessage(HttpMethod.Post, uploadUrl);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        request.Headers.Add("apikey", secretKey);

        request.Content = content;

        var response =
            await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error =
                await response.Content.ReadAsStringAsync(cancellationToken);

            throw new Exception(error);
        }

        return $"{projectUrl}/storage/v1/object/public/{bucket}/{fileName}";
    }

    public async Task DeleteImageAsync(
        string imageUrl,
        CancellationToken cancellationToken = default)
    {
        var projectUrl = _configuration["Supabase:Url"]!;
        var bucket = _configuration["Supabase:Bucket"]!;
        var secretKey = _configuration["Supabase:SecretKey"]!;

        if (string.IsNullOrWhiteSpace(projectUrl) || projectUrl.Contains("projectId.supabase.co", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var fileName =
            Path.GetFileName(new Uri(imageUrl).AbsolutePath);

        var deleteUrl =
            $"{projectUrl}/storage/v1/object/{bucket}/{fileName}";

        using var request =
            new HttpRequestMessage(HttpMethod.Delete, deleteUrl);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", secretKey);

        request.Headers.Add("apikey", secretKey);

        var response =
            await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
