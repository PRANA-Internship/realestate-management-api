using MediatR;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetMySalesById;

public record GetMySalesByIdQuery(
    Guid Id
) : IRequest<Result<UserResponse>>;
