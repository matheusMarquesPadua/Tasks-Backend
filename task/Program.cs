using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Npgsql.EntityFrameworkCore.PostgreSQL;

using Scalar.AspNetCore;
using MediatR;
using FluentValidation;

using task.Src.API.Middlewares;
using task.Src.Infrastructure.Data.Context;
using task.Src.Infrastructure.Services.Email;
using task.Src.Domain.Entities.Identity;
using task.Src.Infrastructure.Services;
using task.Src.Application.Common.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------
// 1. SERVIÇOS (DI - Dependência de Injeção)
// ----------------------------------------------------------------

// Configuração do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));

    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddIdentity < ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? "UmaChaveSuperSecretaMuitoLongaParaOJWTFuncionar");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false; // false = Permite HTTP para desenvolvimento (não recomendado para produção)
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = false,
         ValidateAudience = false,
         ClockSkew = TimeSpan.Zero // Remove o tempo de tolerância para expiração do token
     };
 });

builder.Services.AddAuthorization();


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

app.UseExceptionHandler();
app.UseStatusCodePages();
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
app.MapControllers();

app.Run();