using System.Net.Http.Json;
using Application.Commands;
using Application.DTOs;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Application.Commons;

namespace Test.Controller
{
    public class PermissionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PermissionsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RequestPermission_ReturnsSuccess()
        {
            var request = new RequestPermissionCommand
            {
                EmployeeForename = "Juan",
                EmployeeSurname = "Pérez",
                PermissionTypeId = 1,
                PermissionDate = DateTime.UtcNow
            };

            var response = await _client.PostAsJsonAsync("/api/Permissions/RequestPermission", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPermissions_ReturnsData()
        {
            // Arrange: Crear un permiso para asegurar que hay datos
            await RequestPermission_ReturnsSuccess();

            // Act
            var response = await _client.GetAsync("/api/Permissions/GetPermissions");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadFromJsonAsync<Response<List<PermissionDto>>>();
            content.Should().NotBeNull();
            content!.Success.Should().BeTrue();
            content.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ModifyPermission_ReturnsSuccess()
        {
            // Arrange: Crear un permiso primero
            var createRequest = new RequestPermissionCommand
            {
                EmployeeForename = "Maria",
                EmployeeSurname = "Gomez",
                PermissionTypeId = 1,
                PermissionDate = DateTime.UtcNow
            };

            var createResponse = await _client.PostAsJsonAsync("/api/Permissions/RequestPermission", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<Response<PermissionDto>>();
            var id = created!.Data.Id;

            // Act: Modificar el permiso
            var modifyRequest = new ModifyPermissionCommand
            {
                Id = id,
                EmployeeForename = "Maria",
                EmployeeSurname = "Gómez Actualizada",
                PermissionTypeId = 1,
                PermissionDate = DateTime.UtcNow
            };

            var response = await _client.PutAsJsonAsync($"/api/Permissions/ModifyPermission/{id}", modifyRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var modified = await response.Content.ReadFromJsonAsync<Response<PermissionDto>>();
            modified!.Success.Should().BeTrue();
            modified.Data.EmployeeSurname.Should().Be("Gómez Actualizada");
        }

        [Fact]
        public async Task ModifyPermission_ReturnsBadRequest()
        {
            var request = new ModifyPermissionCommand
            {
                Id = 999,
                EmployeeForename = "Carlos",
                EmployeeSurname = "Contreras",
                PermissionTypeId = 1,
                PermissionDate = DateTime.UtcNow
            };

            var response = await _client.PutAsJsonAsync("/api/Permissions/ModifyPermission/1", request); // ID en la URL diferente

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}