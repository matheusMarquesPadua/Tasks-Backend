using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using task.Src.Infrastructure.Data.Context;
using Scalar.AspNetCore;
using task.Src.Domain.Entities.Identity;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------
// 1. SERVIÇOS (DI - Dependência de Injeção)
// ----------------------------------------------------------------

// Configuração do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configuração de Identidade (Auth)
// O .NET 10 facilita muito o login para SPAs (Vue.js)
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>();

// CORS - Essencial para o Vue.js (Vite) na porta 5173
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // .NET 10 OpenAPI

var app = builder.Build();

// ----------------------------------------------------------------
// 2. PIPELINE DE REQUISIÇÕES (Middlewares)
// ----------------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Aplica a política de CORS
app.UseCors("VueApp");

app.UseAuthentication();
app.UseAuthorization();

// ----------------------------------------------------------------
// 3. MAPEAMENTO DE ROTAS
// ----------------------------------------------------------------

// Este comando cria automaticamente os endpoints de JSON:
// /register, /login, /refresh, /confirmEmail, etc.
app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();