using MediatR;
using RS.Application.Common.Interfaces;
using RS.Application.Features.Reservations.Queries.GetManagedPropertyReservation;
using RS.Contracts.Reservations;
using RS.Domain.Common;
using RS.Domain.Enums;


public class GetManagedPropertyReservationsQueryHandler
    : IRequestHandler<
        GetManagedPropertyReservationsQuery,
        Result<PaginatedResult<ManagedPropertyReservationResponse>>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserContext _userContext;

    public GetManagedPropertyReservationsQueryHandler(
        IReservationRepository reservationRepository,
        IUserContext userContext)
    {
        _reservationRepository = reservationRepository;
        _userContext = userContext;
    }

    public async Task<Result<PaginatedResult<ManagedPropertyReservationResponse>>> Handle(
        GetManagedPropertyReservationsQuery request,
        CancellationToken ct)
    {


        var reservations = await _reservationRepository
            .GetManagedPropertyReservationsAsync(
                _userContext.UserId,
                request.Status,
                request.Search,
                request.Page,
                request.PageSize,
                ct);

        var response = reservations.Data
            .Select(x => new ManagedPropertyReservationResponse
            (
                x.Id,
                x.PropertyId,
                x.Property.Title,
                x.BuyerFullName,
                x.BuyerEmail,
                x.BuyerPhoneNumber,
                x.ReservationFee,
                x.Status,
                x.ReservedAt,
               x.ExpiresAt
            ))
            .ToList();

        return Result<PaginatedResult<ManagedPropertyReservationResponse>>.Success(
            new PaginatedResult<ManagedPropertyReservationResponse>(
                response,
                reservations.Meta));
    }
}
