using MediatR;
using RS.Application.Common.Interfaces;
using RS.Contracts.Users;
using RS.Domain.Common;
using RS.Domain.Enums;

namespace RS.Application.Features.Users.Queries.GetMySalesById;

public class GetMySalesByIdQueryHandler
    : IRequestHandler<GetMySalesByIdQuery, Result<UserResponse>>
{

    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;


    public GetMySalesByIdQueryHandler(
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }



    public async Task<Result<UserResponse>> Handle(
        GetMySalesByIdQuery request,
        CancellationToken ct)
    {

        if (_userContext.Role != UserRole.MANAGER)
        {
            return Result<UserResponse>.Failure(
                new Error(
                    "FORBIDDEN",
                    "Only managers can view sales."));
        }


        var sales =
            await _userRepository.GetSalesByManagerAndIdAsync(
                _userContext.UserId,
                request.Id,
                ct);



        if (sales == null)
        {
            return Result<UserResponse>.Failure(
                new Error(
                    "SALES_NOT_FOUND",
                    "Sales user not found."));
        }



        var response = new UserResponse(
            sales.Id,
            sales.FullName,
            sales.Email,
            sales.Phone,
            sales.Role.ToString(),
            sales.Status.ToString(),
            sales.CreatedAt
        );


        return Result<UserResponse>.Success(response);
    }
}
