using AudiSoft.Application.Common;

namespace AudiSoft.Application.Notas;

public interface INotaService
{
    Task<PagedResultDto<NotaDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize);
    Task<NotaDto> ObtenerPorIdAsync(int id);
    Task<NotaDto> CrearAsync(CrearNotaDto dto);
    Task<NotaDto> ActualizarAsync(int id, ActualizarNotaDto dto);
    Task EliminarAsync(int id);
}
