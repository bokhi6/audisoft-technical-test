using AudiSoft.Application;
using AudiSoft.Infrastructure;
using AudiSoft.Infrastructure.Persistence;
using AudiSoft.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
