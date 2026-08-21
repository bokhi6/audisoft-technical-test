using AudiSoft.Application.Common;
using AudiSoft.Application.Notas;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;

namespace AudiSoft.Application.Estudiantes;

public class EstudianteService : IEstudianteService
{
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly INotaRepository _notaRepository;

    public EstudianteService(IEstudianteRepository estudianteRepository, INotaRepository notaRepository)
    {
        _estudianteRepository = estudianteRepository;
        _notaRepository = notaRepository;
    }

    public async Task<PagedResultDto<EstudianteDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _estudianteRepository.ObtenerPaginadoAsync(pageNumber, pageSize);
        return new PagedResultDto<EstudianteDto>
        {
            Items = items.Select(MapearADto).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<ItemListaDto>> ObtenerListaAsync()
    {
        var estudiantes = await _estudianteRepository.ObtenerTodosAsync();
        return estudiantes.Select(e => new ItemListaDto(e.Id, e.Nombre)).ToList();
    }

    public async Task<EstudianteDto> ObtenerPorIdAsync(int id)
    {
        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el estudiante con id {id}.");
        return MapearADto(estudiante);
    }

    public async Task<EstudianteDto> CrearAsync(CrearEstudianteDto dto)
    {
        ValidarNombre(dto.Nombre);

        var estudiante = new Estudiante { Nombre = dto.Nombre.Trim() };
        await _estudianteRepository.AgregarAsync(estudiante);
        await _estudianteRepository.GuardarCambiosAsync();

        return MapearADto(estudiante);
    }

    public async Task<EstudianteDto> ActualizarAsync(int id, ActualizarEstudianteDto dto)
    {
        ValidarNombre(dto.Nombre);

        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el estudiante con id {id}.");

        estudiante.Nombre = dto.Nombre.Trim();
        _estudianteRepository.Actualizar(estudiante);
        await _estudianteRepository.GuardarCambiosAsync();

        return MapearADto(estudiante);
    }

    public async Task EliminarAsync(int id)
    {
        var estudiante = await _estudianteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el estudiante con id {id}.");

        var tieneNotas = await _notaRepository.ExisteAsociadaAEstudianteAsync(id);
        if (tieneNotas)
        {
            throw new ConflictoDeIntegridadException(
                "No se puede eliminar el estudiante porque tiene notas asociadas.");
        }

        _estudianteRepository.Eliminar(estudiante);
        await _estudianteRepository.GuardarCambiosAsync();
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationAppException("El nombre del estudiante es obligatorio.");
        }

        if (nombre.Trim().Length > 200)
        {
            throw new ValidationAppException("El nombre del estudiante no puede superar 200 caracteres.");
        }
    }

    private static EstudianteDto MapearADto(Estudiante estudiante) => new(estudiante.Id, estudiante.Nombre, estudiante.Notas.Count);
}
