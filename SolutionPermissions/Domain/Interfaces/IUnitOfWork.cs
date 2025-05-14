
namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPermissionRepository PermissionRepository { get; }
        IPermissionTypeRepository PermissionTypeRepository { get; }
        Task<int> CompleteAsync();
    }
}
