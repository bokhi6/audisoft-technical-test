using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;
using AudiSoft.Application.Profesores;
using Microsoft.Extensions.DependencyInjection;

namespace AudiSoft.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IProfesorService, ProfesorService>();
        services.AddScoped<INotaService, NotaService>();

        return services;
    }
}
