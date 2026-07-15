using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Notifications.Commands.MarkNotificationRead;
using RS.Application.Features.Notifications.Queries.GetMyNotifications;

namespace RS.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController(
    IMediator mediator)
    : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken ct)
    {

        var result =
            await mediator.Send(
                new GetMyNotificationsQuery(),
                ct);


        if (result.IsSuccess)
            return Ok(result.Value);


        return BadRequest(result.Error);
    }




    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(
        Guid id,
        CancellationToken ct)
    {

        var result =
            await mediator.Send(
                new MarkNotificationReadCommand(id),
                ct);


        if (result.IsSuccess)
            return NoContent();


        if (result.Error.Code == "NOTIFICATION_NOT_FOUND")
            return NotFound(result.Error);


        if (result.Error.Code == "FORBIDDEN")
            return Forbid();


        return BadRequest(result.Error);
    }
}
