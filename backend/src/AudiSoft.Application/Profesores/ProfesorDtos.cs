namespace AudiSoft.Application.Profesores;

public record ProfesorDto(int Id, string Nombre);

public record CrearProfesorDto(string Nombre);

public record ActualizarProfesorDto(string Nombre);
