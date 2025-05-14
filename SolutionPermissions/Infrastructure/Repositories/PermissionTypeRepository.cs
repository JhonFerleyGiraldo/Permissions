using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly PermissionDbContext _context;

        public PermissionTypeRepository(PermissionDbContext context) => _context = context;

        public async Task AddAsync(PermissionType permission) => await _context.PermissionTypes.AddAsync(permission);
        public async Task<List<PermissionType>> GetAllAsync() => await _context.PermissionTypes.ToListAsync();
        public async Task<PermissionType?> GetByIdAsync(int id) => await _context.PermissionTypes.FirstOrDefaultAsync(p => p.Id == id);
        public void Update(PermissionType permission) => _context.PermissionTypes.Update(permission);
    }
}
