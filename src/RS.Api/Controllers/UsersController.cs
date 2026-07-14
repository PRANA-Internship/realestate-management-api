using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS.Application.Features.Users.Commands.UpdateMySalesStatus;
using RS.Application.Features.Users.Commands.UpdateUser;
using RS.Application.Features.Users.Commands.UpdateUserStatus;
using RS.Application.Features.Users.Queries.GetMySales;
using RS.Application.Features.Users.Queries.GetMySalesById;
using RS.Application.Features.Users.Queries.GetUserById;
using RS.Application.Features.Users.Queries.GetUsers;
using RS.Domain.Enums;

namespace RS.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{


    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] UserRole? role,
        [FromQuery] UserStatus? status,
        [FromQuery] string? search,
        CancellationToken ct,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
        )
    {

        var result = await mediator.Send(
            new GetUsersQuery(
                role,
                status,
                search,
                page,
                pageSize),
            ct);


        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }


        return BadRequest(result.Error);
    }




    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(
        Guid id,
        CancellationToken ct)
    {

        var result = await mediator.Send(
            new GetUserByIdQuery(id),
            ct);


        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }


        if (result.Error.Code == "USER_NOT_FOUND")
        {
            return NotFound(result.Error);
        }


        return BadRequest(result.Error);
    }





    [HttpPut]
    public async Task<IActionResult> UpdateUser(

        [FromBody] UpdateUserCommand command,
        CancellationToken ct)
    {




        var result = await mediator.Send(
            command,
            ct);


        if (result.IsSuccess)
        {
            return Ok(new
            {
                Message = "User updated successfully"
            });
        }


        if (result.Error.Code == "USER_NOT_FOUND")
        {
            return NotFound(result.Error);
        }


        return BadRequest(result.Error);
    }





    [HttpPatch("/status")]
    public async Task<IActionResult> UpdateStatus(

        [FromBody] UpdateUserStatusCommand command,
        CancellationToken ct)
    {


        var result = await mediator.Send(
            command,
            ct);



        if (result.IsSuccess)
        {
            return Ok(new
            {
                Message = "User status updated successfully"
            });
        }


        if (result.Error.Code == "USER_NOT_FOUND")
        {
            return NotFound(result.Error);
        }


        return BadRequest(result.Error);
    }


    [Authorize(Roles = "MANAGER")]
    [HttpGet("my-sales")]
    public async Task<IActionResult> GetMySales(CancellationToken ct, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
   )
    {
        var result = await mediator.Send(
            new GetMySalesQuery(page, pageSize),
            ct);


        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }


        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER")]
    [HttpGet("my-sales/{id:guid}")]
    public async Task<IActionResult> GetMySalesById(
    Guid id,
    CancellationToken ct)
    {

        var result = await mediator.Send(
            new GetMySalesByIdQuery(id),
            ct);


        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }


        if (result.Error.Code == "SALES_NOT_FOUND")
        {
            return NotFound(result.Error);
        }


        return BadRequest(result.Error);
    }

    [Authorize(Roles = "MANAGER")]
    [HttpPatch("my-sales/{id:guid}/status")]
    public async Task<IActionResult> UpdateMySalesStatus(
    Guid id,
    [FromBody] UserStatus status,
    CancellationToken ct)
    {

        var result = await mediator.Send(
            new UpdateMySalesStatusCommand
            {
                SalesId = id,
                Status = status
            },
            ct);



        if (result.IsSuccess)
        {
            return Ok(new
            {
                Message = "Sales status updated successfully"
            });
        }


        if (result.Error.Code == "SALES_NOT_FOUND")
        {
            return NotFound(result.Error);
        }


        return BadRequest(result.Error);
    }
}
