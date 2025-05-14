using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task AddAsync(Permission permission);
        Task<List<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        public void Update(Permission permission);
    }
}
