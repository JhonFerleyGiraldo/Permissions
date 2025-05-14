using Application.Commons;
using Application.DTOs;
using MediatR;

namespace Application.Commands
{

    public class RequestPermissionCommand : IRequest<Response<PermissionDto>>
    {
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
