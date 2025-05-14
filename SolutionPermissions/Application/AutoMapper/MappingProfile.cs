using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PermissionDto, RequestPermissionCommand>();
            CreateMap<PermissionDto, ModifyPermissionCommand>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>();
        }
    }
}
