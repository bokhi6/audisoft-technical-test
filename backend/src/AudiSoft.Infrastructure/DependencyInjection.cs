using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;
using AudiSoft.Application.Profesores;
using AudiSoft.Infrastructure.Persistence;
using AudiSoft.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AudiSoft.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AudiSoftDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IEstudianteRepository, EstudianteRepository>();
        services.AddScoped<IProfesorRepository, ProfesorRepository>();
        services.AddScoped<INotaRepository, NotaRepository>();

        return services;
    }
}
