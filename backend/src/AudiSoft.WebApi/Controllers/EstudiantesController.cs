using AudiSoft.Application.Estudiantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoft.WebApi.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Authorize]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteService _estudianteService;

    public EstudiantesController(IEstudianteService estudianteService)
    {
        _estudianteService = estudianteService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
    {
        var resultado = await _estudianteService.ObtenerPaginadoAsync(pageNumber, pageSize);
        return Ok(resultado);
    }

    [HttpGet("lista")]
    public async Task<IActionResult> ObtenerLista()
    {
        var resultado = await _estudianteService.ObtenerListaAsync();
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var estudiante = await _estudianteService.ObtenerPorIdAsync(id);
        return Ok(estudiante);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearEstudianteDto dto)
    {
        var creado = await _estudianteService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEstudianteDto dto)
    {
        var actualizado = await _estudianteService.ActualizarAsync(id, dto);
        return Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _estudianteService.EliminarAsync(id);
        return NoContent();
    }
}
