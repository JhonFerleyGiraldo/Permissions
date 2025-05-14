using Application.Business.Interfaces;
using Application.Commands;
using Application.Commons;
using Application.DTOs;
using MediatR;

namespace Application.Handler
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, Response<PermissionDto>>
    {
        private readonly IPermissionsBusiness _permissionsBusiness;

        public RequestPermissionHandler(IPermissionsBusiness permissionsBusiness)
        {
            _permissionsBusiness = permissionsBusiness;
        }

        public async Task<Response<PermissionDto>> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            return await _permissionsBusiness.AddAsync(request);
        }
    }
}
