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
using RS.Application.Features.Properties.Queries.GetPublicProperties;
using RS.Application.Features.Properties.Queries.GetPublicPropertyById;
using RS.Domain.Enums;
using RS.Infrastructure.Authentication;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IMediator mediator) : ControllerBase
{


    [AllowAnonymous]
    [HttpGet("public")]
    public async Task<IActionResult> GetPublicProperties(
    [FromQuery] GetPublicPropertiesQuery query,
    CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [AllowAnonymous]
    [HttpGet("public/{id:guid}")]
    public async Task<IActionResult> GetPublicProperty(
    Guid id,
    CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetPublicPropertyByIdQuery(id),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }
    [HasPermission(Permission.CreateProperty)]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePropertyCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
            return Ok(new { PropertyId = result.Value });

        return BadRequest(new { Error = result.Error });
    }

    [HasPermission(Permission.ReadProperties)]
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

    [HasPermission(Permission.ReadMyProperties)]
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

    [HasPermission(Permission.ReadProperty)]
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

    [HasPermission(Permission.UpdateProperty)]
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


    [HasPermission(Permission.DeleteProperty)]
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


    [HasPermission(Permission.AddPropertyImages)]
    [HttpPost("/images")]
    public async Task<IActionResult> AddImages(

    [FromForm] AddPropertyImagesCommand command,
    CancellationToken ct)
    {


        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [HasPermission(Permission.DeletePropertyImage)]
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

    [HasPermission(Permission.SetPrimaryPropertyImage)]
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

    [HasPermission(Permission.ChangePropertyActiveState)]
    [HttpPatch("/active")]
    public async Task<IActionResult> ChangeActiveState(

        [FromBody] ChangePropertyActiveStateCommand command,
        CancellationToken ct)
    {


        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

}
