using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
    .AddApplicationServices()
    .AddInfrastrutureServices(builder.Configuration)
    .AddApiServices();

//------------------
//Infrastructure - EF Core
//Application - MediatR
//API - Carter, HealthChecks, ...

//builder.Services
//    .AddApplicationServices()
//    .AddInfrastructureServices(builder.Configuration)
//    .AddWebServices();
//------------------

var app = builder.Build();

// Configure the HTTP request pipeline

app.Run();
