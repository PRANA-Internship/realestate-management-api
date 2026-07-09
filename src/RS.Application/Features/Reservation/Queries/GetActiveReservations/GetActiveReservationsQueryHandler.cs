using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Reservations;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Reservations.Queries.GetActiveReservations;

public class GetActiveReservationsQueryHandler
    : IRequestHandler<
        GetActiveReservationsQuery,
        Result<IReadOnlyList<ReservationDetailResponse>>>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IReservationRepository _reservationRepository;

    public GetActiveReservationsQueryHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IReservationRepository reservationRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<Result<IReadOnlyList<ReservationDetailResponse>>> Handle(
        GetActiveReservationsQuery request,
        CancellationToken ct)
    {



        var sales = await _userRepository.GetByIdAsync(
            _userContext.UserId,
            ct);

        if (sales == null || sales.CreatedByUserId == null)
        {
            return Result<IReadOnlyList<ReservationDetailResponse>>.Failure(
                new Error(
                    "MANAGER_NOT_FOUND",
                    "Manager not found."));
        }

        var reservations =
            await _reservationRepository.GetAllReservationAsync(
                sales.CreatedByUserId.Value,
                ct);

        var response = reservations
      .Select(r => new ReservationDetailResponse(
          r.Id,
          r.PropertyId,
          r.Property.Title,
          r.Property.Price,
          r.BuyerFullName,
          r.BuyerEmail,
          r.BuyerPhoneNumber,
          r.ReservationFee,
          r.Status.ToString(),
          r.ReservedAt,
          r.ExpiresAt
         ))
      .ToList();

        return Result<IReadOnlyList<ReservationDetailResponse>>
            .Success(response);
    }
}
