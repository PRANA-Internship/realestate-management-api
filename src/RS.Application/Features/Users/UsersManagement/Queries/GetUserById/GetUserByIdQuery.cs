using MediatR;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(
    Guid UserId
) : IRequest<Result<UserDetailResponse>>;
