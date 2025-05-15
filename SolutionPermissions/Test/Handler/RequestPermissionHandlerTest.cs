using Application.Business.Implementations;
using Application;
using Application.Business.Interfaces;
using Application.Commands;
using Application.Commons;
using Application.DTOs;
using Application.Handler;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Queries;
using Microsoft.Data.Sqlite;

namespace Test.Handler
{
    public class RequestPermissionHandlerTests
    {

        private readonly ServiceProvider _serviceProvider;
        private readonly SqliteConnection _sqliteConnection;

        public RequestPermissionHandlerTests()
        {

            _sqliteConnection = new SqliteConnection("Data Source=:memory:;Cache=Shared");
            _sqliteConnection.Open();

            // Construir configuración desde appsettings.json o con valores en memoria
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", "Server=(localdb)\\MSSQLLocalDB;Database=PermissionsTestDb;Trusted_Connection=True;" },
                { "ConnectionStrings:Dapper", "Server=(localdb)\\MSSQLLocalDB;Database=PermissionsTestDb;Trusted_Connection=True;" },
                {"Kafka:BootstrapServers", "localhost:9092"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();

            // Registrar IConfiguration como singleton
            services.AddSingleton<IConfiguration>(configuration);

            // Registrar DbContext con configuración
            services.AddDbContext<PermissionDbContext>(options =>
            options.UseSqlite(_sqliteConnection));

            // AutoMapper
            services.AddAutoMapper(typeof(Application.AutoMapper.MappingProfile));

            // Mock de servicios externos
            var mockKafka = new Mock<IKafkaProducer<OperationDto>>();
            var mockElastic = new Mock<IElasticsearchService>();
            var mockLogger = new Mock<ILogger<PermissionsBusiness>>();

            services.AddSingleton(mockKafka.Object);
            services.AddSingleton(mockElastic.Object);
            services.AddSingleton(mockLogger.Object);

            // Repositorios y UoW
            services.AddScoped<IDapperConnection, DapperConnection>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Servicios
            services.AddScoped<IPermissionsBusiness, PermissionsBusiness>();
            services.AddScoped<RequestPermissionHandler>();
            services.AddScoped<ModifyPermissionHandler>();
            services.AddScoped<GetPermissionsQueryHandler>();

            _serviceProvider = services.BuildServiceProvider();

            // Seed inicial
            var context = _serviceProvider.GetRequiredService<PermissionDbContext>();
            context.Database.EnsureCreated();

            var permissionType = context.PermissionTypes.Where(x => x.Id == 1).FirstOrDefault();

            if (permissionType == null)
            {
                context.PermissionTypes.Add(new Domain.Entities.PermissionType
                {
                    Id = 1,
                    Description = "Administrador"
                });
                context.SaveChanges();
            }
            
        }

        #region RequestPermissionHandler
        [Fact]
        public async Task Handle_CreatePermission_Success()
        {
            try
            {
                // Arrange
                var handler = _serviceProvider.GetRequiredService<RequestPermissionHandler>();

                var request = new RequestPermissionCommand
                {
                    EmployeeForename = "Jhon",
                    EmployeeSurname = "Giraldo",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.UtcNow
                };

                // Act
                var result = await handler.Handle(request, default);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Jhon", result.Data.EmployeeForename);
            }
            catch (Exception ex)
            {
                throw;
            }  
        }

        [Fact]
        public async Task Handle_CreatePermission_Fail()
        {
            try
            {
                // Arrange
                var handler = _serviceProvider.GetRequiredService<RequestPermissionHandler>();

                var request = new RequestPermissionCommand
                {
                    EmployeeForename = "Jhon",
                    EmployeeSurname = "Giraldo",
                    PermissionTypeId = 0, //Id no existe
                    PermissionDate = DateTime.UtcNow
                };

                // Act
                var result = await handler.Handle(request, default);

                // Assert
                Assert.False(result.Success);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ModifyPermissionHandler

        private async Task<int> SavePermission()
        {
            try
            {
                // Arrange
                var handler = _serviceProvider.GetRequiredService<RequestPermissionHandler>();

                var request = new RequestPermissionCommand
                {
                    EmployeeForename = "Jhon",
                    EmployeeSurname = "Giraldo",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.UtcNow
                };

                var result = await handler.Handle(request, default);

                return result.Data.Id;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Fact]
        public async Task Handle_ModifyPermission_Success()
        {
            try
            {

                var idPermission = await this.SavePermission();

                // Arrange
                var handler = _serviceProvider.GetRequiredService<ModifyPermissionHandler>();

                var request = new ModifyPermissionCommand
                {
                    Id = idPermission,
                    EmployeeForename = "Jhon",
                    EmployeeSurname = "Giraldo",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.UtcNow
                };

                // Act
                var result = await handler.Handle(request, default);

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Jhon", result.Data.EmployeeForename);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Fact]
        public async Task Handle_ModifyPermission_Fail()
        {
            try
            {
                // Arrange
                var handler = _serviceProvider.GetRequiredService<ModifyPermissionHandler>();

                var request = new ModifyPermissionCommand
                {
                    Id = 0,
                    EmployeeForename = "Jhon",
                    EmployeeSurname = "Giraldo",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.UtcNow
                };

                // Act
                var result = await handler.Handle(request, default);

                // Assert
                Assert.False(result.Success);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetPermissionsQueryHandler
        [Fact]
        public async Task Handle_GetPermission_Success()
        {
            try
            {

                var idPermission = await this.SavePermission();

                // Arrange
                var handler = _serviceProvider.GetRequiredService<GetPermissionsQueryHandler>();

                var request = new GetPermissionsQuery();

                // Act
                var result = await handler.Handle(request, default);

                // Assert
                Assert.True(result.Success);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        public void Dispose()
        {
            _sqliteConnection.Close();
            _sqliteConnection.Dispose();
        }

    }
}
