using Application.Commons;
using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetPermissionsQuery : IRequest<Response<List<PermissionDto>>> { }
}
