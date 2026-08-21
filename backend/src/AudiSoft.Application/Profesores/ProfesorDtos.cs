namespace AudiSoft.Application.Profesores;

public record ProfesorDto(int Id, string Nombre, int CantidadNotas);

public record CrearProfesorDto(string Nombre);

public record ActualizarProfesorDto(string Nombre);
