using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Dashboard;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Dashboard.Queries.GetAdminDashboard;

public class GetAdminDashboardQueryHandler
    : IRequestHandler<GetAdminDashboardQuery, Result<AdminDashboardResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IReservationRepository _reservationRepository;


    public GetAdminDashboardQueryHandler(
        IUserRepository userRepository,
        IPropertyRepository propertyRepository,
        IReservationRepository reservationRepository)
    {
        _userRepository = userRepository;
        _propertyRepository = propertyRepository;
        _reservationRepository = reservationRepository;
    }


    public async Task<Result<AdminDashboardResponse>> Handle(
      GetAdminDashboardQuery request,
      CancellationToken ct)
    {
        var users = new UserStatistics(
            await _userRepository.CountAsync((UserRole?)null, ct),
            await _userRepository.CountAsync(UserRole.MANAGER, ct),
            await _userRepository.CountAsync(UserRole.SALES, ct),
            await _userRepository.CountAsync(UserRole.BUYER, ct)
        );


        var properties = new PropertyStatistics(
            await _propertyRepository.CountAsync((PropertyStatus?)null, ct),
            await _propertyRepository.CountAsync(PropertyStatus.Available, ct),
            await _propertyRepository.CountAsync(PropertyStatus.Reserved, ct),
            await _propertyRepository.CountAsync(PropertyStatus.Sold, ct),
            await _propertyRepository.CountAsync(PropertyStatus.Rented, ct),
            await _propertyRepository.CountAsync(PropertyStatus.UnderMaintenance, ct),
            await _propertyRepository.CountAsync(PropertyStatus.Inactive, ct)
        );


        var reservations = new ReservationStatistics(
            await _reservationRepository.CountAsync((ReservationStatus?)null, ct),
            await _reservationRepository.CountAsync(ReservationStatus.PendingPayment, ct),
            await _reservationRepository.CountAsync(ReservationStatus.Reserved, ct),
            await _reservationRepository.CountAsync(ReservationStatus.Expired, ct)
        );


        return Result<AdminDashboardResponse>.Success(
            new AdminDashboardResponse(
                users,
                properties,
                reservations
            ));
    }
}
