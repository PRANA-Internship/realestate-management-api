using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Auth.Commands.Login;
using RS.Application.Features.Auth.Commands.RegisterBuyer;
using RS.Application.Features.Users.Commands.CreateManager;
using RS.Application.Features.Users.Commands.SetPassword;

namespace RS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        [Authorize(Roles = "ADMIN")]
        [HttpPost("managers")]
        public async Task<IActionResult> CreateManager(
            [FromBody] CreateManagerCommand command,
            CancellationToken ct)
        {
            var result = await mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    ManagerId = result.Value,
                    Message = "Manager created successfully."
                });
            }

            return BadRequest(result.Error);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("sales")]
        public async Task<IActionResult> CreateSales(
    CreateSalesCommand command,
    CancellationToken ct)
        {
            var result = await mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    SalesId = result.Value
                });
            }

            return BadRequest(result.Error);
        }

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


        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword(
        SetPasswordCommand command,
        CancellationToken ct)
        {
            var result = await mediator.Send(command, ct);

            if (result.IsSuccess)
                return Ok(new { Message = "Account activated successfully" });

            return BadRequest(result.Error);
        }
    }
}

