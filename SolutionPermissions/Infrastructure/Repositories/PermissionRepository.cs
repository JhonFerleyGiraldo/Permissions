using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionDbContext _context;
        private readonly IDapperConnection _dapperConnection;

        public PermissionRepository(PermissionDbContext context, IDapperConnection dapperConnection)
        {
            _context = context;
            _dapperConnection = dapperConnection;
        }
        public async Task AddAsync(Permission permission) 
        {
            await _context.Permissions.AddAsync(permission);
        }
        public async Task<List<Permission>> GetAllAsync()
        {
            return await _dapperConnection.GetAllPermissionsAsync();
            return new();
        }
        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions.Include(p => p.PermissionType).FirstOrDefaultAsync(p => p.Id == id);
        }
        public void Update(Permission permission)
        {
            _context.Permissions.Update(permission);
        }
    }
}
