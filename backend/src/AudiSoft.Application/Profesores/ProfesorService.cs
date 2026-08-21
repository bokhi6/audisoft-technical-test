using AudiSoft.Application.Common;
using AudiSoft.Application.Notas;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;

namespace AudiSoft.Application.Profesores;

public class ProfesorService : IProfesorService
{
    private readonly IProfesorRepository _profesorRepository;
    private readonly INotaRepository _notaRepository;

    public ProfesorService(IProfesorRepository profesorRepository, INotaRepository notaRepository)
    {
        _profesorRepository = profesorRepository;
        _notaRepository = notaRepository;
    }

    public async Task<PagedResultDto<ProfesorDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _profesorRepository.ObtenerPaginadoAsync(pageNumber, pageSize);
        return new PagedResultDto<ProfesorDto>
        {
            Items = items.Select(MapearADto).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<ItemListaDto>> ObtenerListaAsync()
    {
        var profesores = await _profesorRepository.ObtenerTodosAsync();
        return profesores.Select(p => new ItemListaDto(p.Id, p.Nombre)).ToList();
    }

    public async Task<ProfesorDto> ObtenerPorIdAsync(int id)
    {
        var profesor = await _profesorRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el profesor con id {id}.");
        return MapearADto(profesor);
    }

    public async Task<ProfesorDto> CrearAsync(CrearProfesorDto dto)
    {
        ValidarNombre(dto.Nombre);

        var profesor = new Profesor { Nombre = dto.Nombre.Trim() };
        await _profesorRepository.AgregarAsync(profesor);
        await _profesorRepository.GuardarCambiosAsync();

        return MapearADto(profesor);
    }

    public async Task<ProfesorDto> ActualizarAsync(int id, ActualizarProfesorDto dto)
    {
        ValidarNombre(dto.Nombre);

        var profesor = await _profesorRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el profesor con id {id}.");

        profesor.Nombre = dto.Nombre.Trim();
        _profesorRepository.Actualizar(profesor);
        await _profesorRepository.GuardarCambiosAsync();

        return MapearADto(profesor);
    }

    public async Task EliminarAsync(int id)
    {
        var profesor = await _profesorRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el profesor con id {id}.");

        var tieneNotas = await _notaRepository.ExisteAsociadaAProfesorAsync(id);
        if (tieneNotas)
        {
            throw new ConflictoDeIntegridadException(
                "No se puede eliminar el profesor porque tiene notas asociadas.");
        }

        _profesorRepository.Eliminar(profesor);
        await _profesorRepository.GuardarCambiosAsync();
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationAppException("El nombre del profesor es obligatorio.");
        }

        if (nombre.Trim().Length > 200)
        {
            throw new ValidationAppException("El nombre del profesor no puede superar 200 caracteres.");
        }
    }

    private static ProfesorDto MapearADto(Profesor profesor) => new(profesor.Id, profesor.Nombre);
}
