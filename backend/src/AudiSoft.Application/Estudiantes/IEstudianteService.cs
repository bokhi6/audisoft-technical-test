using AudiSoft.Application.Common;

namespace AudiSoft.Application.Estudiantes;

public interface IEstudianteService
{
    Task<PagedResultDto<EstudianteDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<List<ItemListaDto>> ObtenerListaAsync();
    Task<EstudianteDto> ObtenerPorIdAsync(int id);
    Task<EstudianteDto> CrearAsync(CrearEstudianteDto dto);
    Task<EstudianteDto> ActualizarAsync(int id, ActualizarEstudianteDto dto);
    Task EliminarAsync(int id);
}
