using System.Text;
using AudiSoft.Application;
using AudiSoft.Infrastructure;
using AudiSoft.Infrastructure.Auth;
using AudiSoft.Infrastructure.Persistence;
using AudiSoft.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

const string PoliticaCorsAngular = "AngularDev";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "AudiSoft API",
        Version = "v1",
        Description = "API REST para la gestión de Estudiantes, Profesores y Notas."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresar el token obtenido en /api/auth/login (sin el prefijo 'Bearer')."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(PoliticaCorsAngular, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSection = builder.Configuration.GetSection(JwtOptions.SeccionConfiguracion);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"] ?? string.Empty))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Aplica migraciones pendientes automáticamente al arrancar (idempotente).
// Simplifica tanto el arranque local como en contenedores Docker: no hace
// falta correr "dotnet ef database update" a mano.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AudiSoftDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger habilitado siempre (no solo en Development) para facilitar
// la prueba del API con Postman, como recomienda la prueba técnica.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AudiSoft API v1");
});

app.UseCors(PoliticaCorsAngular);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
