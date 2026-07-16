using MediatR;
using RS.Domain.Common;

namespace RS.Application.Features.Users.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    string? FullName,
    string? Phone)
    : IRequest<Result>;
