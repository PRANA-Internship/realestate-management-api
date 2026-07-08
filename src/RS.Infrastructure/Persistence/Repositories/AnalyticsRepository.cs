using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RS.Application.Common.Interfaces;
using RS.Application.Features.Analytics.Queries;

namespace RS.Infrastructure.Persistence.Repositories;

public class AnalyticsRepository(RSDbContext dbContext) : IAnalyticsRepository
{
    public async Task<AnalyticsResponse> GetAnalyticsAsync(DateTime? startDate, DateTime? endDate, CancellationToken ct = default)
    {
        var connection = dbContext.Database.GetDbConnection();
        var wasOpen = connection.State == ConnectionState.Open;
        if (!wasOpen)
        {
            await connection.OpenAsync(ct);
        }

        try
        {
            var overview = await QueryOverviewAsync(connection, startDate, endDate, ct);
            var agentPerformance = await QueryAgentPerformanceAsync(connection, startDate, endDate, ct);
            var propertyTypePerformance = await QueryPropertyTypePerformanceAsync(connection, startDate, endDate, ct);
            var timeSeriesTrend = await QueryTimeSeriesTrendAsync(connection, startDate, endDate, ct);

            return new AnalyticsResponse(overview, agentPerformance, propertyTypePerformance, timeSeriesTrend);
        }
        finally
        {
            if (!wasOpen)
            {
                await connection.CloseAsync();
            }
        }
    }

    private static DbCommand CreateCommand(DbConnection connection, string sql, DateTime? startDate, DateTime? endDate)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        if (startDate.HasValue)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = "@startDate";
            p.Value = DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc);
            p.DbType = DbType.DateTime;
            cmd.Parameters.Add(p);
        }

        if (endDate.HasValue)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = "@endDate";
            p.Value = DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc);
            p.DbType = DbType.DateTime;
            cmd.Parameters.Add(p);
        }

        return cmd;
    }

    private async Task<OverviewAnalyticsDto> QueryOverviewAsync(DbConnection connection, DateTime? startDate, DateTime? endDate, CancellationToken ct)
    {
        var dateFilter = GetDateFilter("r.\"ReservedAt\"", startDate, endDate);
        var sql = $@"
            SELECT 
                COUNT(*)::int AS TotalReservations,
                COALESCE(SUM(CASE WHEN r.""Status"" = 'Reserved' THEN r.""ReservationFee"" ELSE 0 END), 0) AS TotalRevenue,
                COALESCE(AVG(CASE WHEN r.""Status"" = 'Reserved' THEN r.""ReservationFee"" ELSE NULL END), 0) AS AverageReservationFee,
                COUNT(CASE WHEN r.""Status"" = 'PendingPayment' THEN 1 END)::int AS PendingPaymentCount,
                COUNT(CASE WHEN r.""Status"" = 'Reserved' THEN 1 END)::int AS ReservedCount,
                COUNT(CASE WHEN r.""Status"" = 'Expired' THEN 1 END)::int AS ExpiredCount
            FROM ""Reservations"" r
            {dateFilter}";

        using var cmd = CreateCommand(connection, sql, startDate, endDate);
        using var reader = await cmd.ExecuteReaderAsync(ct);

        if (await reader.ReadAsync(ct))
        {
            return new OverviewAnalyticsDto(
                reader.GetInt32(0),
                reader.GetDecimal(1),
                reader.GetDecimal(2),
                reader.GetInt32(3),
                reader.GetInt32(4),
                reader.GetInt32(5)
            );
        }

        return new OverviewAnalyticsDto(0, 0, 0, 0, 0, 0);
    }

    private async Task<List<AgentSalesAnalyticsDto>> QueryAgentPerformanceAsync(DbConnection connection, DateTime? startDate, DateTime? endDate, CancellationToken ct)
    {
        var dateFilter = GetDateFilter("r.\"ReservedAt\"", startDate, endDate);
        var sql = $@"
            SELECT 
                u.""Id"" AS AgentId,
                u.""FullName"" AS AgentName,
                COUNT(r.""Id"")::int AS ReservationsCount,
                COALESCE(SUM(r.""ReservationFee""), 0) AS TotalRevenue
            FROM ""Reservations"" r
            JOIN ""Properties"" p ON r.""PropertyId"" = p.""Id""
            JOIN ""Users"" u ON p.""CreatedByUserId"" = u.""Id""
            {dateFilter} {(string.IsNullOrEmpty(dateFilter) ? "WHERE" : "AND")} r.""Status"" = 'Reserved'
            GROUP BY u.""Id"", u.""FullName""
            ORDER BY TotalRevenue DESC";

        using var cmd = CreateCommand(connection, sql, startDate, endDate);
        using var reader = await cmd.ExecuteReaderAsync(ct);
        var results = new List<AgentSalesAnalyticsDto>();

        while (await reader.ReadAsync(ct))
        {
            results.Add(new AgentSalesAnalyticsDto(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetDecimal(3)
            ));
        }

        return results;
    }

    private async Task<List<PropertyTypeAnalyticsDto>> QueryPropertyTypePerformanceAsync(DbConnection connection, DateTime? startDate, DateTime? endDate, CancellationToken ct)
    {
        var dateFilter = GetDateFilter("r.\"ReservedAt\"", startDate, endDate);
        var sql = $@"
            SELECT 
                p.""Type"" AS PropertyType,
                COUNT(r.""Id"")::int AS ReservationsCount,
                COALESCE(SUM(r.""ReservationFee""), 0) AS TotalRevenue
            FROM ""Reservations"" r
            JOIN ""Properties"" p ON r.""PropertyId"" = p.""Id""
            {dateFilter} {(string.IsNullOrEmpty(dateFilter) ? "WHERE" : "AND")} r.""Status"" = 'Reserved'
            GROUP BY p.""Type""
            ORDER BY TotalRevenue DESC";

        using var cmd = CreateCommand(connection, sql, startDate, endDate);
        using var reader = await cmd.ExecuteReaderAsync(ct);
        var results = new List<PropertyTypeAnalyticsDto>();

        while (await reader.ReadAsync(ct))
        {
            results.Add(new PropertyTypeAnalyticsDto(
                reader.GetString(0),
                reader.GetInt32(1),
                reader.GetDecimal(2)
            ));
        }

        return results;
    }

    private async Task<List<TimeAnalyticsDto>> QueryTimeSeriesTrendAsync(DbConnection connection, DateTime? startDate, DateTime? endDate, CancellationToken ct)
    {
        var dateFilter = GetDateFilter("r.\"ReservedAt\"", startDate, endDate);
        var sql = $@"
            SELECT 
                TO_CHAR(r.""ReservedAt"", 'YYYY-MM') AS Period,
                COUNT(r.""Id"")::int AS ReservationsCount,
                COALESCE(SUM(r.""ReservationFee""), 0) AS TotalRevenue
            FROM ""Reservations"" r
            {dateFilter} {(string.IsNullOrEmpty(dateFilter) ? "WHERE" : "AND")} r.""Status"" = 'Reserved'
            GROUP BY TO_CHAR(r.""ReservedAt"", 'YYYY-MM')
            ORDER BY Period ASC";

        using var cmd = CreateCommand(connection, sql, startDate, endDate);
        using var reader = await cmd.ExecuteReaderAsync(ct);
        var results = new List<TimeAnalyticsDto>();

        while (await reader.ReadAsync(ct))
        {
            results.Add(new TimeAnalyticsDto(
                reader.GetString(0),
                reader.GetInt32(1),
                reader.GetDecimal(2)
            ));
        }

        return results;
    }

    private static string GetDateFilter(string columnName, DateTime? startDate, DateTime? endDate)
    {
        if (startDate.HasValue && endDate.HasValue)
        {
            return $"WHERE {columnName} >= @startDate AND {columnName} <= @endDate";
        }
        if (startDate.HasValue)
        {
            return $"WHERE {columnName} >= @startDate";
        }
        if (endDate.HasValue)
        {
            return $"WHERE {columnName} <= @endDate";
        }
        return string.Empty;
    }
}
