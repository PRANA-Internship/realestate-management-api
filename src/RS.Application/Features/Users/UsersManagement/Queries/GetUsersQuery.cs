
using MediatR;
using RS.Contracts.Users;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(
    UserRole? Role,
    UserStatus? Status,
    string? Search,
    int Page = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedResult<UserResponse>>>;
