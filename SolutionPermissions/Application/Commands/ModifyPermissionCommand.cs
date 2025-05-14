using Application.Commons;
using Application.DTOs;
using MediatR;

namespace Application.Commands
{
    public class ModifyPermissionCommand : IRequest<Response<PermissionDto>>
    {
        public int Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
