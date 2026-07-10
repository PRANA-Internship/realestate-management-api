using MediatR;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetAvailableProperties;

public class GetAvailablePropertiesQuery
    : IRequest<Result<IReadOnlyList<PropertyResponse>>>
{
}
