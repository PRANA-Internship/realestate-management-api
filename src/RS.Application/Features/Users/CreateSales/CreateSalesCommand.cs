using MediatR;
using RS.Domain.Common;

public class CreateSalesCommand : IRequest<Result<Guid>>
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
}
