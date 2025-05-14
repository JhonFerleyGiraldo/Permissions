using Application.Business.Interfaces;
using Application.Commands;
using Application.Commons;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Business.Implementations
{
    public class PermissionsBusiness : IPermissionsBusiness
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IElasticsearchService _elasticsearchService;
        private readonly IKafkaProducer<OperationDto> _producer;
        private readonly ILogger<PermissionsBusiness> _logger;

        public PermissionsBusiness( IUnitOfWork unitOfWork,
                                    IElasticsearchService elasticsearchService, 
                                    IMapper mapper,
                                    IKafkaProducer<OperationDto> producer,
                                    ILogger<PermissionsBusiness> logger)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _mapper = mapper;
            _producer = producer;
            _logger = logger;
        }

        public async Task<Response<PermissionDto>> AddAsync(RequestPermissionCommand request)
        {
            Response<PermissionDto> response;
            try
            {
                #region Serilog
                _logger.LogInformation("Operación: Registrando permisos");
                #endregion

                #region kafka

                var dto = new OperationDto()
                {
                    Id = Guid.NewGuid(),
                    OperationName = "request"
                };

                await _producer.SendMessageAsync(dto);

                #endregion

                var permissionType = await _unitOfWork.PermissionTypeRepository.GetByIdAsync(request.PermissionTypeId);

                if (permissionType == null)
                {
                    response = new(null, false, $"El tipo de permiso con Id:{request.PermissionTypeId} no existe.");
                    return response;
                }

                var permission = new Permission
                {
                    EmployeeForename = request.EmployeeForename,
                    EmployeeSurname = request.EmployeeSurname,
                    PermissionTypeId = request.PermissionTypeId,
                    PermissionDate = request.PermissionDate
                };

                await _unitOfWork.PermissionRepository.AddAsync(permission);
                await _unitOfWork.CompleteAsync();

                #region Elasticsearch
                var permisoParaElastic = new PermissionDto
                {
                    Id = permission.Id,
                    EmployeeForename = permission.EmployeeForename,
                    EmployeeSurname = permission.EmployeeSurname,
                    PermissionTypeId = permission.PermissionTypeId,
                    PermissionDate = permission.PermissionDate
                };

                await _elasticsearchService.IndexPermissionAsync(permisoParaElastic);
                #endregion

                response = new(_mapper.Map<PermissionDto>(permission));
            }
            catch (Exception ex)
            {

                response = new(null, false, ex.Message);
            }

            return response;
        }

        public async Task<Response<List<PermissionDto>>> GetAllAsync()
        {
            Response<List<PermissionDto>> response;

            try
            {

                #region Serilog
                _logger.LogInformation("Operación: Obteniendo permisos");
                #endregion

                #region kafka

                var dto = new OperationDto()
                {
                    Id = Guid.NewGuid(),
                    OperationName = "get"
                };

                await _producer.SendMessageAsync(dto);

                #endregion

                var permissions = await _unitOfWork.PermissionRepository.GetAllAsync();
                response = new(_mapper.Map<List<PermissionDto>>(permissions));
            }
            catch (Exception ex)
            {
                response = new(null, false, ex.Message);
            }

            return response;
        }

        public Task<Response<PermissionDto?>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<PermissionDto>> Update(ModifyPermissionCommand request)
        {
            Response<PermissionDto> response;

            try
            {

                #region Serilog
                _logger.LogInformation("Operación: Actualizando permisos");
                #endregion

                #region kafka

                var dto = new OperationDto()
                {
                    Id = Guid.NewGuid(),
                    OperationName = "modify"
                };

                await _producer.SendMessageAsync(dto);

                #endregion

                var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(request.Id);

                if (permission == null)
                {
                    response = new(null, false, $"El permiso con Id:{request.Id} no existe.");
                    return response;
                }

                var permissionType = await _unitOfWork.PermissionTypeRepository.GetByIdAsync(request.PermissionTypeId);

                if (permissionType == null)
                {
                    response = new(null, false, $"El tipo de permiso con Id:{request.PermissionTypeId} no existe.");
                    return response;
                }

                permission.EmployeeForename = request.EmployeeForename;
                permission.EmployeeSurname = request.EmployeeSurname;
                permission.PermissionTypeId = request.PermissionTypeId;
                permission.PermissionDate = request.PermissionDate;

                _unitOfWork.PermissionRepository.Update(permission);
                await _unitOfWork.CompleteAsync();

                #region Elasticsearch
                var permisoParaElastic = new PermissionDto
                {
                    Id = permission.Id,
                    EmployeeForename = permission.EmployeeForename,
                    EmployeeSurname = permission.EmployeeSurname,
                    PermissionTypeId = permission.PermissionTypeId,
                    PermissionDate = permission.PermissionDate
                };
                await _elasticsearchService.IndexPermissionAsync(permisoParaElastic);
                #endregion
                response = new(_mapper.Map<PermissionDto>(permission));

            }
            catch (Exception ex)
            {
                response = new(null, false, ex.Message);
            }

            return response;
        }
    }
}
