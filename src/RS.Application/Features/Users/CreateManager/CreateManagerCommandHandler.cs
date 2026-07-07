using MediatR;
using Microsoft.Extensions.Configuration;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.CreateManager;

public class CreateManagerCommandHandler
    : IRequestHandler<CreateManagerCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public CreateManagerCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext, IEmailService emailService,
    IConfiguration configuration,
    IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _emailService = emailService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateManagerCommand request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.ADMIN)
        {
            return Result<Guid>.Failure(
                new Error("FORBIDDEN", "Only admin can create manager."));
        }

        var exists = await _userRepository.GetByEmailAsync(request.Email, ct);

        if (exists != null)
        {
            return Result<Guid>.Failure(
                new Error("EMAIL_EXISTS", "Email already exists."));
        }


        var admin = await _userRepository.GetByIdAsync(_userContext.UserId, ct);

        if (admin == null)
        {
            return Result<Guid>.Failure(
                new Error("ADMIN_NOT_FOUND", "Admin not found."));
        }


        var manager = User.CreateStaff(
            request.FullName,
            request.Email,
            request.Phone,
            UserRole.MANAGER);

        manager.SetCreatedBy(admin.Id);
        var token = manager.GeneratePasswordResetToken();

        manager.SetStatus(UserStatus.INACTIVE);


        await _userRepository.AddAsync(manager, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var frontendUrl = _configuration["AppSettings:FrontendUrl"];

        var activationUrl =
            $"{frontendUrl}/set-password?token={token}";

        await _emailService.SendPasswordSetupAsync(
    manager.Email,
    manager.FullName,
    activationUrl,
    ct);
        return Result<Guid>.Success(manager.Id);
    }
}
