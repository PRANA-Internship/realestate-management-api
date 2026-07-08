using System;
using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Payments.Commands.InitiatePayment;

public record InitiatePaymentCommand(Guid ReservationId) : IRequest<Result<string>>;
