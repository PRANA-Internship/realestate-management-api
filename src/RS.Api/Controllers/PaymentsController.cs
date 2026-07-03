using MediatR;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Payments.Commands.InitiatePayment;
using RS.Application.Features.Payments.Commands.VerifyPayment;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController(ISender mediator) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate(InitiatePaymentCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpGet("verify/{txRef}")]
    public async Task<IActionResult> Verify(string txRef, CancellationToken ct)
    {
        var verified = await mediator.Send(new VerifyPaymentCommand(txRef), ct);
        return verified ? Ok(new { status = "success" }) : BadRequest(new { status = "failed" });
    }
}
