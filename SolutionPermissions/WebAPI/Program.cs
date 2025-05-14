using Application.DTOs;
using Application;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Application.Commands;
using Application.Business.Interfaces;
using Application.Business.Implementations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configuracion Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/logPermissions-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Configuracion EF Core con SQL Server
builder.Services.AddDbContext<PermissionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configuracion CQRS
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RequestPermissionCommand).Assembly));


//Configuracion Elasticsearch
builder.Services.Configure<ElasticsearchSettingsDto>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

//configuracion Kafka
builder.Services.AddSingleton<IKafkaProducer<OperationDto>>(provider =>
    new KafkaProducer<OperationDto>("operaciones-usuario", builder.Configuration["Kafka:BootstrapServers"]));


builder.Services.AddAutoMapper(typeof(Application.AutoMapper.MappingProfile));


//Inyeccion dependencias
builder.Services.AddScoped<IPermissionsBusiness, PermissionsBusiness>();
builder.Services.AddScoped<IDapperConnection, DapperConnection>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
