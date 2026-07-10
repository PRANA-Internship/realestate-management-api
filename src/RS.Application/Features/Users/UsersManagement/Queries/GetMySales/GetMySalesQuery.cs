using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetMySales;

public record GetMySalesQuery
    : IRequest<Result<IReadOnlyCollection<UserResponse>>>;
