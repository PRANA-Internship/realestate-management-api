using System;
using System.Collections.Generic;

namespace RS.Application.Features.Analytics.Queries;

public record OverviewAnalyticsDto(
    int TotalReservations,
    decimal TotalRevenue,
    decimal AverageReservationFee,
    int PendingPaymentCount,
    int ReservedCount,
    int ExpiredCount
);

public record AgentSalesAnalyticsDto(
    Guid AgentId,
    string AgentName,
    int ReservationsCount,
    decimal TotalRevenue
);

public record PropertyTypeAnalyticsDto(
    string PropertyType,
    int ReservationsCount,
    decimal TotalRevenue
);

public record TimeAnalyticsDto(
    string Period,
    int ReservationsCount,
    decimal TotalRevenue
);

public record AnalyticsResponse(
    OverviewAnalyticsDto Overview,
    List<AgentSalesAnalyticsDto> AgentPerformance,
    List<PropertyTypeAnalyticsDto> PropertyTypePerformance,
    List<TimeAnalyticsDto> TimeSeriesTrend
);
