using Application.Business.Interfaces;
using Application.Commands;
using Application.Commons;
using Application.DTOs;
using MediatR;

namespace Application.Handler
{
    public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, Response<PermissionDto>>
    {
        private readonly IPermissionsBusiness _permissionsBusiness;

        public ModifyPermissionHandler(IPermissionsBusiness permissionsBusiness)
        {
            _permissionsBusiness = permissionsBusiness;
        }

        public async Task<Response<PermissionDto>> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsBusiness.Update(request);
        }
    }
}
