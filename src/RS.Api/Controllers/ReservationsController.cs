using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Reservations.Commands.CreateReservation;
using RS.Application.Features.Reservations.Queries.GetActiveReservations;
using RS.Application.Features.Reservations.Queries.GetMyReservations;
using RS.Application.Features.Reservations.Queries.GetReservationById;

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

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyReservations(
       CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetMyReservationsQuery(),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetReservation(
        Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetReservationByIdQuery(id),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error.Code == "RESERVATION_NOT_FOUND")
        {
            return NotFound(result.Error);
        }

        if (result.Error.Code == "FORBIDDEN")
        {
            return Forbid();
        }

        return BadRequest(result.Error);
    }

    [Authorize]
    [HttpGet("sales/active")]
    public async Task<IActionResult> GetAllReservationforSales(
    CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetActiveReservationsQuery(),
            ct);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Error);
    }
}
