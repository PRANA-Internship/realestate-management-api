using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid PropertyId,
    string? BuyerFullName,
    string? BuyerEmail,
    string? BuyerPhoneNumber)
    : IRequest<Result<Guid>>;
