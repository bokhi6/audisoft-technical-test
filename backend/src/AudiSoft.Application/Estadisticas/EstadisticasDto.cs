namespace AudiSoft.Application.Estadisticas;

public record EstadisticasDto(
    decimal PromedioGeneral,
    int TotalNotas,
    int NotasAprobadas,
    int NotasReprobadas,
    int TotalEstudiantes,
    int EstudiantesAprobados,
    int EstudiantesReprobados,
    int EstudiantesSinNotas,
    decimal PorcentajeEstudiantesAprobados,
    decimal PorcentajeEstudiantesReprobados);
