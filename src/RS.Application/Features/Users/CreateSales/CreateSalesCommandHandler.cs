using MediatR;
using Microsoft.Extensions.Configuration;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;
using RS.Domain.Entities;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Commands.CreateManager;

public class CreateSalesCommandHandler
    : IRequestHandler<CreateSalesCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSalesCommandHandler(
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
        CreateSalesCommand request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.MANAGER)
        {
            return Result<Guid>.Failure(
                new Error("FORBIDDEN", "Only Manager can create sales account."));
        }

        var exists = await _userRepository.GetByEmailAsync(request.Email, ct);

        if (exists != null)
        {
            return Result<Guid>.Failure(
                new Error("EMAIL_EXISTS", "Email already exists."));
        }


        var manager = await _userRepository.GetByIdAsync(_userContext.UserId, ct);

        if (manager == null)
        {
            return Result<Guid>.Failure(
                new Error("MANAGER_NOT_FOUND", "Manager not found."));
        }


        var sales = User.CreateStaff(
            request.FullName,
            request.Email,
            request.Phone,
            UserRole.SALES);

        sales.SetCreatedBy(manager.Id);
        var token = sales.GeneratePasswordResetToken();

        sales.SetStatus(UserStatus.INACTIVE);


        await _userRepository.AddAsync(sales, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var frontendUrl = _configuration["AppSettings:FrontendUrl"];

        var activationUrl =
            $"{frontendUrl}/set-password?token={token}";

        await _emailService.SendPasswordSetupAsync(
    sales.Email,
    sales.FullName,
    activationUrl,
    ct);
        return Result<Guid>.Success(sales.Id);
    }
}
