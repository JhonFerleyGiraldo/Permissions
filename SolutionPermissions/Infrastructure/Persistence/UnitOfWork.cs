using Domain.Interfaces;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PermissionDbContext _context;
        public IPermissionRepository PermissionRepository { get; }
        public IPermissionTypeRepository PermissionTypeRepository { get; }

        public UnitOfWork(  PermissionDbContext context,
                            IPermissionRepository permissionRepository,
                            IPermissionTypeRepository permissionTypeRepository)
        {
            _context = context;
            PermissionRepository = permissionRepository;
            PermissionTypeRepository = permissionTypeRepository;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
