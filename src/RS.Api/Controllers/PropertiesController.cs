using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Properties.Command.CreateProperty;
using RS.Application.Features.Properties.Commands.AddPropertyImages;
using RS.Application.Features.Properties.Commands.ChangePropertyActiveState;
using RS.Application.Features.Properties.Commands.DeleteProperty;
using RS.Application.Features.Properties.Commands.DeletePropertyImage;
using RS.Application.Features.Properties.Commands.SetPrimaryPropertyImage;
using RS.Application.Features.Properties.Commands.UpdateProperty;
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

    [Authorize(Roles = "MANAGER,ADMIN")]
    [HttpPut]
    public async Task<IActionResult> Update(

    [FromBody] UpdatePropertyCommand command,
    CancellationToken ct)
    {


        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);


    }


    [Authorize(Roles = "MANAGER,ADMIN")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeletePropertyCommand(id),
            ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }


    [Authorize(Roles = "MANAGER")]
    [HttpPost("{id:guid}/images")]
    public async Task<IActionResult> AddImages(
    Guid id,
    [FromForm] AddPropertyImagesCommand command,
    CancellationToken ct)
    {
        command.PropertyId = id;

        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER")]
    [HttpDelete("images/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(
        Guid imageId,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeletePropertyImageCommand(imageId),
            ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER")]
    [HttpPatch("images/{imageId:guid}/primary")]
    public async Task<IActionResult> SetPrimaryImage(
        Guid imageId,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new SetPrimaryPropertyImageCommand(imageId),
            ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER")]
    [HttpPatch("{id:guid}/active")]
    public async Task<IActionResult> ChangeActiveState(
        Guid id,
        [FromBody] ChangePropertyActiveStateCommand command,
        CancellationToken ct)
    {
        command.PropertyId = id;

        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

}
