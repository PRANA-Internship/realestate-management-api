using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Payments.Commands.InitiatePayment;
using RS.Application.Features.Payments.Commands.VerifyPayment;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IMediator mediator) : ControllerBase
{
    [HttpPost("initialize/{reservationId:guid}")]
    public async Task<IActionResult> Initialize(Guid reservationId, CancellationToken ct)
    {
        var result = await mediator.Send(new InitiatePaymentCommand(reservationId), ct);

        if (result.IsSuccess)
        {
            return Ok(new { CheckoutUrl = result.Value });
        }

        return BadRequest(new { Error = result.Error });
    }

    [HttpGet("verify/{txRef}")]
    public async Task<IActionResult> Verify(string txRef, CancellationToken ct)
    {
        var result = await mediator.Send(new VerifyPaymentCommand(txRef), ct);

        if (result.IsSuccess)
        {
            return Ok(new { Verified = result.Value });
        }

        return BadRequest(new { Error = result.Error });
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] ChapaWebhookPayload payload, CancellationToken ct)
    {
        if (payload == null || string.IsNullOrEmpty(payload.TxRef))
        {
            return BadRequest("Invalid payload");
        }

        var result = await mediator.Send(new VerifyPaymentCommand(payload.TxRef), ct);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(new { Error = result.Error });
    }
}

public class ChapaWebhookPayload
{
    [JsonPropertyName("event")]
    public string Event { get; set; } = default!;

    [JsonPropertyName("tx_ref")]
    public string TxRef { get; set; } = default!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;
}
