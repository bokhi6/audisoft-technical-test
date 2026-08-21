using AudiSoft.Application.Common;

namespace AudiSoft.Application.Profesores;

public interface IProfesorService
{
    Task<PagedResultDto<ProfesorDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<List<ItemListaDto>> ObtenerListaAsync();
    Task<ProfesorDto> ObtenerPorIdAsync(int id);
    Task<ProfesorDto> CrearAsync(CrearProfesorDto dto);
    Task<ProfesorDto> ActualizarAsync(int id, ActualizarProfesorDto dto);
    Task EliminarAsync(int id);
}
