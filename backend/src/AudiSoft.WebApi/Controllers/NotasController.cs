using AudiSoft.Application.Notas;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoft.WebApi.Controllers;

[ApiController]
[Route("api/notas")]
public class NotasController : ControllerBase
{
    private readonly INotaService _notaService;

    public NotasController(INotaService notaService)
    {
        _notaService = notaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
    {
        var resultado = await _notaService.ObtenerPaginadoAsync(pageNumber, pageSize);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var nota = await _notaService.ObtenerPorIdAsync(id);
        return Ok(nota);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearNotaDto dto)
    {
        var creada = await _notaService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarNotaDto dto)
    {
        var actualizada = await _notaService.ActualizarAsync(id, dto);
        return Ok(actualizada);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _notaService.EliminarAsync(id);
        return NoContent();
    }
}
