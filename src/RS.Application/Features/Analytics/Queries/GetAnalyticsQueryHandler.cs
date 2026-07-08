using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Analytics.Queries;

public class GetAnalyticsQueryHandler : IRequestHandler<GetAnalyticsQuery, Result<AnalyticsResponse>>
{
    private readonly IAnalyticsRepository _analyticsRepository;

    public GetAnalyticsQueryHandler(IAnalyticsRepository analyticsRepository)
    {
        _analyticsRepository = analyticsRepository;
    }

    public async Task<Result<AnalyticsResponse>> Handle(GetAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var response = await _analyticsRepository.GetAnalyticsAsync(request.StartDate, request.EndDate, cancellationToken);
        return Result<AnalyticsResponse>.Success(response);
    }
}
