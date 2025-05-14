using Application.Business.Interfaces;
using Application.Commons;
using Application.DTOs;
using Application.Queries;
using MediatR;

namespace Application.Handler
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Response<List<PermissionDto>>>
    {
        private readonly IPermissionsBusiness _permissionsBusiness;

        public GetPermissionsQueryHandler(IPermissionsBusiness permissionsBusiness)
        {
            _permissionsBusiness = permissionsBusiness;
        }

        public async Task<Response<List<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _permissionsBusiness.GetAllAsync();
        }
    }
}
