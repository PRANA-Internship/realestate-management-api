using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Reservations.Commands.CreateReservation;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IMediator mediator) : ControllerBase
{

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReservationCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            return Ok(new
            {
                ReservationId = result.Value
            });
        }

        return BadRequest(new
        {
            Error = result.Error
        });
    }
}
