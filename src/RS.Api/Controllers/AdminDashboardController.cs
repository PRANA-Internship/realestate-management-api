using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Dashboard.Queries.GetAdminDashboard;

namespace RS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class AdminDashboardController(IMediator mediator) : ControllerBase
{

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetAdminDashboardQuery(),
            ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
