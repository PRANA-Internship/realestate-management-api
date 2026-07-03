using MediatR;
using Microsoft.AspNetCore.Http;
using RS.Domain.Common;

namespace RS.Application.Features.Properties.Commands.AddPropertyImages;

public class AddPropertyImagesCommand : IRequest<Result>
{
    public Guid PropertyId { get; set; }

    public List<IFormFile> Images { get; set; } = [];
}
