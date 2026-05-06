using Microsoft.AspNetCore.Identity;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using task.Src.Infrastructure.Data.Context;
using Scalar.AspNetCore;
using task.Src.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi;
using task.Src.Infrastructure.Services.Email;
using task.Src.Infrastructure.Services;

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

// Configuração de E-mail
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Configuração de App
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));


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
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Define o esquema de segurança (Cadeado)
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Insira o Token JWT gerado no /login"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = scheme;

        // 2. Aplica a exigência de segurança globalmente no documento
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});

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