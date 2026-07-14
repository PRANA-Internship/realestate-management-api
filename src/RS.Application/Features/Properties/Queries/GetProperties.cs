using MediatR;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetProperties;

public sealed record GetPropertiesQuery(
    string? City,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Type,
    int Page = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedResult<PropertyResponse>>>;
