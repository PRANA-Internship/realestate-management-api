

using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery
    : IRequest<Result<ProfileResponse>>;
