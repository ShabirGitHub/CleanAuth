using CleanAuth.Infrastructure.Repositories;
using CleanAuth.Infrastructure.Services;
using CleanAuth.Infrastructure.UnitOfWork;
using CleanAuth.UseCases;
using CleanAuth.UseCases.Interfaces;
using CleanAuth.UseCases.RepositoryPlugins;
using CleanAuth.UseCases.ServicesPlugins;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader();
        });
});

var connectionString = builder.Configuration.GetConnectionString("Default");

// Register IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(connectionString));

// Register services
builder.Services.AddScoped<ISignupUseCase, SignupUseCase>();
builder.Services.AddScoped<IAuthenticateUseCase, AuthenticateUseCase>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IBalanceUseCase, BalanceUseCase>();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Unit of work
builder.Services.AddScoped<IUnitOfWorkFactory, TransactionScopeUnitOfWorkFactory>();

builder.Services.AddControllers(); 

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
