using AudiSoft.Application.Common;
using AudiSoft.Application.Estudiantes;
using AudiSoft.Application.Profesores;
using AudiSoft.Domain.Entities;
using AudiSoft.Domain.Exceptions;

namespace AudiSoft.Application.Notas;

public class NotaService : INotaService
{
    private const decimal ValorMinimo = 0m;
    private const decimal ValorMaximo = 5m;

    private readonly INotaRepository _notaRepository;
    private readonly IEstudianteRepository _estudianteRepository;
    private readonly IProfesorRepository _profesorRepository;

    public NotaService(
        INotaRepository notaRepository,
        IEstudianteRepository estudianteRepository,
        IProfesorRepository profesorRepository)
    {
        _notaRepository = notaRepository;
        _estudianteRepository = estudianteRepository;
        _profesorRepository = profesorRepository;
    }

    public async Task<PagedResultDto<NotaDto>> ObtenerPaginadoAsync(int pageNumber, int pageSize)
    {
        var (items, totalCount) = await _notaRepository.ObtenerPaginadoAsync(pageNumber, pageSize);
        return new PagedResultDto<NotaDto>
        {
            Items = items.Select(MapearADto).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<NotaDto> ObtenerPorIdAsync(int id)
    {
        var nota = await _notaRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró la nota con id {id}.");
        return MapearADto(nota);
    }

    public async Task<NotaDto> CrearAsync(CrearNotaDto dto)
    {
        await ValidarAsync(dto.Nombre, dto.Valor, dto.IdEstudiante, dto.IdProfesor);

        var nota = new Nota
        {
            Nombre = dto.Nombre.Trim(),
            IdEstudiante = dto.IdEstudiante,
            IdProfesor = dto.IdProfesor,
            Valor = dto.Valor
        };
        await _notaRepository.AgregarAsync(nota);
        await _notaRepository.GuardarCambiosAsync();

        var creada = await _notaRepository.ObtenerPorIdAsync(nota.Id)
            ?? throw new NotFoundException("No se pudo recuperar la nota recién creada.");
        return MapearADto(creada);
    }

    public async Task<NotaDto> ActualizarAsync(int id, ActualizarNotaDto dto)
    {
        await ValidarAsync(dto.Nombre, dto.Valor, dto.IdEstudiante, dto.IdProfesor);

        var nota = await _notaRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró la nota con id {id}.");

        nota.Nombre = dto.Nombre.Trim();
        nota.IdEstudiante = dto.IdEstudiante;
        nota.IdProfesor = dto.IdProfesor;
        nota.Valor = dto.Valor;

        _notaRepository.Actualizar(nota);
        await _notaRepository.GuardarCambiosAsync();

        var actualizada = await _notaRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException("No se pudo recuperar la nota actualizada.");
        return MapearADto(actualizada);
    }

    public async Task EliminarAsync(int id)
    {
        var nota = await _notaRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException($"No se encontró la nota con id {id}.");

        _notaRepository.Eliminar(nota);
        await _notaRepository.GuardarCambiosAsync();
    }

    private async Task ValidarAsync(string nombre, decimal valor, int idEstudiante, int idProfesor)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ValidationAppException("El nombre de la nota es obligatorio.");
        }

        if (nombre.Trim().Length > 200)
        {
            throw new ValidationAppException("El nombre de la nota no puede superar 200 caracteres.");
        }

        if (valor < ValorMinimo || valor > ValorMaximo)
        {
            throw new ValidationAppException($"El valor de la nota debe estar entre {ValorMinimo} y {ValorMaximo}.");
        }

        if (!await _estudianteRepository.ExisteAsync(idEstudiante))
        {
            throw new NotFoundException("El estudiante indicado no existe.");
        }

        if (!await _profesorRepository.ExisteAsync(idProfesor))
        {
            throw new NotFoundException("El profesor indicado no existe.");
        }
    }

    private static NotaDto MapearADto(Nota nota) => new(
        nota.Id,
        nota.Nombre,
        nota.IdEstudiante,
        nota.Estudiante?.Nombre ?? string.Empty,
        nota.IdProfesor,
        nota.Profesor?.Nombre ?? string.Empty,
        nota.Valor);
}
