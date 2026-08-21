namespace AudiSoft.Application.Estudiantes;

public record EstudianteDto(int Id, string Nombre, int CantidadNotas);

public record CrearEstudianteDto(string Nombre);

public record ActualizarEstudianteDto(string Nombre);
