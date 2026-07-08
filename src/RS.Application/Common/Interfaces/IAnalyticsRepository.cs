using System;
using System.Threading;
using System.Threading.Tasks;
using RS.Application.Features.Analytics.Queries;

namespace RS.Application.Common.Interfaces;

public interface IAnalyticsRepository
{
    Task<AnalyticsResponse> GetAnalyticsAsync(DateTime? startDate, DateTime? endDate, CancellationToken ct = default);
}
