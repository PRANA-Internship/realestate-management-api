using MediatR;
using RS.Domain.Common;
using RS.Contracts.Properties;

namespace RS.Application.Features.Properties.Queries.GetProperties;

public sealed record GetPropertiesQuery(
    string? City,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Type,
    int Page = 1,
    int PageSize = 10
) : IRequest<Result<List<PropertyResponse>>>;
