using AudiSoft.Domain.Entities;

namespace AudiSoft.Application.Estudiantes;

public interface IEstudianteRepository
{
    Task<(List<Estudiante> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<List<Estudiante>> ObtenerTodosAsync();
    Task<Estudiante?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task AgregarAsync(Estudiante estudiante);
    void Actualizar(Estudiante estudiante);
    void Eliminar(Estudiante estudiante);
    Task GuardarCambiosAsync();
}
