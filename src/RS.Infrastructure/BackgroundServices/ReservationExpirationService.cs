using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RS.Application.Common.Interfaces;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Infrastructure.Services;

public class ReservationExpirationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReservationExpirationService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var reservationRepository =
                scope.ServiceProvider.GetRequiredService<IReservationRepository>();

            var configurationService =
                scope.ServiceProvider.GetRequiredService<IConfigurationService>();

            var unitOfWork =
                scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var intervalSeconds =
                await configurationService.GetIntAsync(
                    "Reservation.ExpirationCheckIntervalSeconds",
                    stoppingToken);
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            if (intervalSeconds <= 0)
            {
                intervalSeconds = 60;
            }

            var expiredReservations =
                await reservationRepository.GetExpiredPendingReservationsAsync(
                    stoppingToken);

            foreach (var reservation in expiredReservations)
            {
                reservation.Status = ReservationStatus.Expired;

                reservationRepository.Update(reservation);
            }

            if (expiredReservations.Count > 0)
            {
                await unitOfWork.SaveChangesAsync(stoppingToken);

                foreach (var reservation in expiredReservations)
                {
                    if (reservation.BuyerUserId is Guid buyerUserId)
                    {
                        await notificationService.NotifyAsync(
                            buyerUserId,
                            "Reservation Expired",
                            $"Your reservation for '{reservation.Property.Title}' has expired.",
                            stoppingToken);
                    }
                }

            }

            await Task.Delay(
                TimeSpan.FromSeconds(intervalSeconds),
                stoppingToken);
        }
    }
}
