using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Properties.Command.CreateProperty;
using RS.Application.Features.Properties.Queries.GetMyProperties;
using RS.Application.Features.Properties.Queries.GetProperties;
using RS.Application.Features.Properties.Queries.GetPropertyById;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "ADMIN,MANAGER")]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePropertyCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
            return Ok(new { PropertyId = result.Value });

        return BadRequest(new { Error = result.Error });
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetPropertiesQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER,ADMIN")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyProperties(
        [FromQuery] GetMyPropertiesQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "ADMIN,MANAGER")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetPropertyByIdQuery(id), ct);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }
}
