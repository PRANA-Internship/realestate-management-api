using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Configurations.Commands.UpdateConfiguration;
using RS.Application.Features.Configurations.Queries.GetConfigurationByKey;
using RS.Application.Features.Configurations.Queries.GetConfigurations;
using RS.Domain.Enums;
using RS.Infrastructure.Authentication;
namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[HasPermission(Permission.ConfigurationManage)]
public class ConfigurationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetConfigurationsQuery(),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetByKey(
        string key,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetConfigurationByKeyQuery(key),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> Update(
        string key,
        [FromBody] UpdateConfigurationRequest request,
        CancellationToken ct)
    {
        var command = new UpdateConfigurationCommand(
            key,
            request.Value);

        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}

public record UpdateConfigurationRequest(string Value);
