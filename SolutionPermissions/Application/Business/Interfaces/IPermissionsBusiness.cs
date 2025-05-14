using Application.Commands;
using Application.Commons;
using Application.DTOs;

namespace Application.Business.Interfaces
{
    public interface IPermissionsBusiness
    {
        Task<Response<PermissionDto>> AddAsync(RequestPermissionCommand request);
        Task<Response<List<PermissionDto>>> GetAllAsync();
        Task<Response<PermissionDto?>> GetByIdAsync(int id);
        Task<Response<PermissionDto>> Update(ModifyPermissionCommand request);
    }
}
