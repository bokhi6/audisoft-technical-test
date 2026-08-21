namespace AudiSoft.Application.Notas;

public record NotaDto(
    int Id,
    string Nombre,
    int IdEstudiante,
    string NombreEstudiante,
    int IdProfesor,
    string NombreProfesor,
    decimal Valor);

public record CrearNotaDto(string Nombre, int IdEstudiante, int IdProfesor, decimal Valor);

public record ActualizarNotaDto(string Nombre, int IdEstudiante, int IdProfesor, decimal Valor);
