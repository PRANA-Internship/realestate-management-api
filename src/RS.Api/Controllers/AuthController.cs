using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Auth.Commands.Login;
using RS.Application.Features.Auth.Commands.RegisterBuyer;

namespace RS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register/buyer")]
        public async Task<IActionResult> RegisterBuyer([FromBody] RegisterBuyerCommand command, CancellationToken ct)
        {
            var result = await mediator.Send(command, ct);
            if (result.IsSuccess)
                return Ok(new { Message = "Buyer registered successfully." });

            return BadRequest(new { Error = result.Error });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await mediator.Send(command, ct);
            if (result.IsSuccess)
                return Ok(result.Value);

            return Unauthorized(new { Error = result.Error });
        }
    }
}
