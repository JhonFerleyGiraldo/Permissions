using Application.DTOs;

namespace Application
{
    public interface IElasticsearchService
    {
        Task IndexPermissionAsync(PermissionDto permissionDto);
    }
}
