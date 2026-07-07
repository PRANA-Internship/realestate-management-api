using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<IReadOnlyCollection<UserResponse>>>
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


    public async Task<Result<IReadOnlyCollection<UserResponse>>> Handle(
        GetUsersQuery request,
        CancellationToken ct)
    {

        // Only ADMIN can manage users
        if (_userContext.Role != Domain.Enums.UserRole.ADMIN)
        {
            return Result<IReadOnlyCollection<UserResponse>>
                .Failure(
                    new Error(
                        "FORBIDDEN",
                        "Only admin can view users."));
        }


        var users = await _userRepository.GetUsersAsync(
            request.Role,
            request.Status,
            request.Search,
            ct);


        var response = users
            .Select(x => new UserResponse(
                x.Id,
                x.FullName,
                x.Email,
                x.Phone,
                x.Role.ToString(),
                x.Status.ToString(),
                x.CreatedAt))
            .ToList();


        return Result<IReadOnlyCollection<UserResponse>>
            .Success(response);
    }
}
