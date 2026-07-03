using MediatR;
using Microsoft.AspNetCore.Http;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Command.CreateProperty;

public sealed record CreatePropertyCommand(
    string Title,
    string Description,
    string Type,
    string Status,
    decimal Price,
    string Location,
    string City,
    string Country,
    int Bedrooms,
    int Bathrooms,
    double AreaSize,
    List<IFormFile> Images
) : IRequest<Result<Guid>>;
