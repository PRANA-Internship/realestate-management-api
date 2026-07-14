using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetMySales;

public record GetMySalesQuery(int Page = 1, int PageSize = 10)
    : IRequest<Result<PaginatedResult<UserResponse>>>;
