using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<PaginatedResult<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;


    public GetUsersQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }


    public async Task<Result<PaginatedResult<UserResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken ct)
    {

        // Only ADMIN can manage users
        if (_userContext.Role != Domain.Enums.UserRole.ADMIN)
        {
            return Result<PaginatedResult<UserResponse>>
                .Failure(
                    new Error(
                        "FORBIDDEN",
                        "Only admin can view users."));
        }


        var users = await _userRepository.GetUsersAsync(
            request.Role,
            request.Status,
            request.Search,
            request.Page,
            request.PageSize,
            ct);


        var response = users.Data
            .Select(x => new UserResponse(
                x.Id,
                x.FullName,
                x.Email,
                x.Phone,
                x.Role.ToString(),
                x.Status.ToString(),
                x.CreatedAt))
            .ToList();


        return Result<PaginatedResult<UserResponse>>
            .Success(new PaginatedResult<UserResponse>(response, users.Meta));
    }
}
