using Microsoft.AspNetCore.Http;

namespace RS.Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadImageAsync(
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task DeleteImageAsync(
        string imageUrl,
        CancellationToken cancellationToken = default);
}
