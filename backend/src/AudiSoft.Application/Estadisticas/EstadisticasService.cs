using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Notas;

namespace AudiSoft.Application.Estadisticas;

public class EstadisticasService : IEstadisticasService
{
    private const decimal ValorMinimoAprobado = 3m;

    private readonly INotaRepository _notaRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public EstadisticasService(INotaRepository notaRepository, IEstudianteRepository estudianteRepository)
    {
        _notaRepository = notaRepository;
        _estudianteRepository = estudianteRepository;
    }

    public async Task<EstadisticasDto> ObtenerResumenAsync()
    {
        var notas = await _notaRepository.ObtenerTodasAsync();
        var estudiantes = await _estudianteRepository.ObtenerTodosAsync();

        var totalNotas = notas.Count;
        var promedioGeneral = totalNotas > 0 ? Math.Round(notas.Average(n => n.Valor), 2) : 0m;
        var notasAprobadas = notas.Count(n => n.Valor >= ValorMinimoAprobado);
        var notasReprobadas = totalNotas - notasAprobadas;

        var promediosPorEstudiante = notas
            .GroupBy(n => n.IdEstudiante)
            .Select(g => g.Average(n => n.Valor))
            .ToList();

        var estudiantesAprobados = promediosPorEstudiante.Count(promedio => promedio >= ValorMinimoAprobado);
        var estudiantesReprobados = promediosPorEstudiante.Count - estudiantesAprobados;
        var estudiantesConNotas = promediosPorEstudiante.Count;
        var totalEstudiantes = estudiantes.Count;
        var estudiantesSinNotas = totalEstudiantes - estudiantesConNotas;

        var porcentajeAprobados = estudiantesConNotas > 0
            ? Math.Round(estudiantesAprobados * 100m / estudiantesConNotas, 1)
            : 0m;
        var porcentajeReprobados = estudiantesConNotas > 0
            ? Math.Round(estudiantesReprobados * 100m / estudiantesConNotas, 1)
            : 0m;

        return new EstadisticasDto(
            promedioGeneral,
            totalNotas,
            notasAprobadas,
            notasReprobadas,
            totalEstudiantes,
            estudiantesAprobados,
            estudiantesReprobados,
            estudiantesSinNotas,
            porcentajeAprobados,
            porcentajeReprobados);
    }
}
