using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using RS.Contracts.Dashboard;
using RS.Domain.Common;

namespace RS.Application.Features.Dashboard.Queries.GetAdminDashboard
{
    public record GetAdminDashboardQuery : IRequest<Result<AdminDashboardResponse>>;

}
