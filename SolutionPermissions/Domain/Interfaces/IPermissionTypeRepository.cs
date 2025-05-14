using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPermissionTypeRepository
    {
        Task AddAsync(PermissionType permission);
        Task<List<PermissionType>> GetAllAsync();
        Task<PermissionType?> GetByIdAsync(int id);
        public void Update(PermissionType permission);
    }
}
