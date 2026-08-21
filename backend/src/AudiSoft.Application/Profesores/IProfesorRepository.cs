using AudiSoft.Domain.Entities;

namespace AudiSoft.Application.Profesores;

public interface IProfesorRepository
{
    Task<(List<Profesor> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<List<Profesor>> ObtenerTodosAsync();
    Task<Profesor?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task AgregarAsync(Profesor profesor);
    void Actualizar(Profesor profesor);
    void Eliminar(Profesor profesor);
    Task GuardarCambiosAsync();
}
