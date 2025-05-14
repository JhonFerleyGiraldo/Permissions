using Application;
using Application.Commands;
using Application.DTOs;
using Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(   IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RequestPermission")]
        public async Task<IActionResult> RequestPermission(RequestPermissionCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("ModifyPermission/{id}")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID no coincide con el permiso enviado");

            command.Id = id;
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpGet("GetPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permisos = await _mediator.Send(new GetPermissionsQuery());
            return Ok(permisos);
        }
    }
}
