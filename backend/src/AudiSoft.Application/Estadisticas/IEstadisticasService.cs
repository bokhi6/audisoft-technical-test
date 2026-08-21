namespace AudiSoft.Application.Estadisticas;

public interface IEstadisticasService
{
    Task<EstadisticasDto> ObtenerResumenAsync();
}
