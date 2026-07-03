using System.Collections.Generic;
using MediatR;
using RS.Contracts.Properties;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Queries.GetMyProperties;

public record GetMyPropertiesQuery(
    int Page = 1,
    int PageSize = 10
) : IRequest<Result<IReadOnlyList<PropertySummaryResponse>>>;
