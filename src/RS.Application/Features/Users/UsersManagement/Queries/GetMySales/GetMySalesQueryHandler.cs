using System.Text;
using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Queries.GetMySales;

public class GetMySalesQueryHandler
    : IRequestHandler<GetMySalesQuery,
        Result<PaginatedResult<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;


    public GetMySalesQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }


    public async Task<Result<PaginatedResult<UserResponse>>> Handle(
        GetMySalesQuery request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.MANAGER)
        {
            return Result<PaginatedResult<UserResponse>>
                .Failure(
                new Error(
                    "FORBIDDEN",
                    "Only managers can view sales."));
        }


        var sales =
            await _userRepository.GetSalesByManagerAsync(
                _userContext.UserId,
                request.Page, request.PageSize,
                ct);


        var response = sales.Data
            .Select(x => new UserResponse(
                x.Id,
                x.FullName,
                x.Email,
                x.Phone,
                x.Role.ToString(),
                x.Status.ToString(),
                x.CreatedAt))
            .ToList();


        return Result<PaginatedResult<UserResponse>>
            .Success(new PaginatedResult<UserResponse>(response, sales.Meta));
    }
}
