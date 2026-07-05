using MediatR;
using RS.Application.Common.Interfaces;
using RS.Domain.Common;

namespace RS.Application.Features.Configurations.Commands.UpdateConfiguration;

public class UpdateConfigurationCommandHandler
    : IRequestHandler<UpdateConfigurationCommand, Result>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConfigurationCommandHandler(
        IConfigurationRepository configurationRepository,
        IUnitOfWork unitOfWork)
    {
        _configurationRepository = configurationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateConfigurationCommand request,
        CancellationToken ct)
    {
        var configuration = await _configurationRepository.GetByKeyAsync(
            request.Key,
            ct);

        if (configuration is null)
        {
            return Result.Failure(
                new Error(
                    "CONFIGURATION_001",
                    "Configuration was not found."));
        }

        configuration.Value = request.Value;
        configuration.UpdatedAt = DateTime.UtcNow;

        _configurationRepository.Update(configuration);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
