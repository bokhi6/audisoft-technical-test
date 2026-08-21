namespace AudiSoft.Application.Estudiantes;

public record EstudianteDto(int Id, string Nombre);

public record CrearEstudianteDto(string Nombre);

public record ActualizarEstudianteDto(string Nombre);
