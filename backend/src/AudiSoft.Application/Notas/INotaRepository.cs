using AudiSoft.Domain.Entities;

namespace AudiSoft.Application.Notas;

public interface INotaRepository
{
    Task<(List<Nota> Items, int TotalCount)> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<List<Nota>> ObtenerTodasAsync();
    Task<Nota?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteAsociadaAEstudianteAsync(int idEstudiante);
    Task<bool> ExisteAsociadaAProfesorAsync(int idProfesor);
    Task AgregarAsync(Nota nota);
    void Actualizar(Nota nota);
    void Eliminar(Nota nota);
    Task GuardarCambiosAsync();
}
