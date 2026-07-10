using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, Result<UserDetailResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;


    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }


    public async Task<Result<UserDetailResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken ct)
    {

        // Admin only
        if (_userContext.Role != UserRole.ADMIN)
        {
            return Result<UserDetailResponse>.Failure(
                new Error(
                    "FORBIDDEN",
                    "Only admin can view user details."));
        }


        var user = await _userRepository.GetByIdWithDetailsAsync(
            request.UserId,
            ct);


        if (user == null)
        {
            return Result<UserDetailResponse>.Failure(
                new Error(
                    "USER_NOT_FOUND",
                    "User was not found."));
        }


        var response = new UserDetailResponse(
            user.Id,
            user.FullName,
            user.Email,
            user.Phone,
            user.Role.ToString(),
            user.Status.ToString(),
            user.CreatedByUserId,
            user.CreatedAt,
            user.UpdatedAt);


        return Result<UserDetailResponse>
            .Success(response);
    }
}
