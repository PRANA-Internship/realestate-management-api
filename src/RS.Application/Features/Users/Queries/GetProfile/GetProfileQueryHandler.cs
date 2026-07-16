

using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileResponse>>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetProfileQueryHandler(IUserContext userContext, IUserRepository userRepository)
        {

            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Result<ProfileResponse>> Handle
            (
            GetProfileQuery request,

            CancellationToken ct

            )
        {
            var user = await _userRepository.GetByIdAsync(_userContext.UserId, ct);
            if (user == null)
            {
                return Result<ProfileResponse>.Failure(
                    new Error(
                        "USER_NOT_FOUND",
                        "User not found."));
            }

            return Result<ProfileResponse>.Success(
            new ProfileResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            });
        }

    }
}
