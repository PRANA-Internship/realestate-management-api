using MediatR;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetPublicPropertyById;

public record GetPublicPropertyByIdQuery(
    Guid PropertyId)
    : IRequest<Result<PublicPropertyDetailResponse>>;
