using System;
using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Analytics.Queries;

public record GetAnalyticsQuery(DateTime? StartDate, DateTime? EndDate) : IRequest<Result<AnalyticsResponse>>;
