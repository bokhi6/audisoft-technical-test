using AudiSoft.Application.Profesores;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoft.WebApi.Controllers;

[ApiController]
[Route("api/profesores")]
public class ProfesoresController : ControllerBase
{
    private readonly IProfesorService _profesorService;

    public ProfesoresController(IProfesorService profesorService)
    {
        _profesorService = profesorService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
    {
        var resultado = await _profesorService.ObtenerPaginadoAsync(pageNumber, pageSize);
        return Ok(resultado);
    }

    [HttpGet("lista")]
    public async Task<IActionResult> ObtenerLista()
    {
        var resultado = await _profesorService.ObtenerListaAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var profesor = await _profesorService.ObtenerPorIdAsync(id);
        return Ok(profesor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearProfesorDto dto)
    {
        var creado = await _profesorService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProfesorDto dto)
    {
        var actualizado = await _profesorService.ActualizarAsync(id, dto);
        return Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _profesorService.EliminarAsync(id);
        return NoContent();
    }
}
