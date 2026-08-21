using AudiSoft.Application;
using AudiSoft.Infrastructure;
using AudiSoft.WebApi.Middleware;

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
