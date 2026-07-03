using MediatR;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetPropertyById;

public record GetPropertyByIdQuery(Guid Id)
    : IRequest<Result<PropertyResponse>>;
