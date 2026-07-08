using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Payments.Commands.VerifyPayment;

public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, Result<bool>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IChapaService _chapaService;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyPaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IReservationRepository reservationRepository,
        IChapaService chapaService,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _reservationRepository = reservationRepository;
        _chapaService = chapaService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        var txRef = request.TxRef;
        if (txRef.StartsWith("mock-", StringComparison.OrdinalIgnoreCase))
        {
            txRef = txRef.Substring(5);
        }

        var payment = await _paymentRepository.GetByTxRefAsync(txRef, cancellationToken);
        if (payment is null)
        {
            return Result<bool>.Failure(new Error("PAYMENT_NOT_FOUND", "Payment transaction not found."));
        }

        if (payment.Status == "Completed")
        {
            return Result<bool>.Success(true);
        }

        var response = await _chapaService.VerifyTransactionAsync(txRef, cancellationToken);

        if (response.Status != "success" || response.Data is null)
        {
            payment.Status = "Failed";
            payment.UpdatedAt = DateTime.UtcNow;
            _paymentRepository.Update(payment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.Failure(new Error("PAYMENT_VERIFICATION_FAILED", response.Message ?? "Payment verification failed."));
        }

        if (response.Data.Status == "success")
        {
            payment.Status = "Completed";
            payment.ChapaReference = response.Data.Reference;
            payment.UpdatedAt = DateTime.UtcNow;

            var reservation = payment.Reservation;
            if (reservation is not null)
            {
                reservation.Status = ReservationStatus.Reserved;
                _reservationRepository.Update(reservation);

                if (reservation.Property is not null)
                {
                    reservation.Property.Status = PropertyStatus.Reserved;
                }
            }

            _paymentRepository.Update(payment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        else if (response.Data.Status == "failed")
        {
            payment.Status = "Failed";
            payment.UpdatedAt = DateTime.UtcNow;
            _paymentRepository.Update(payment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(false);
        }

        return Result<bool>.Success(false);
    }
}
