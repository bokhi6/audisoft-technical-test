using AudiSoft.Application.Estadisticas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudiSoft.WebApi.Controllers;

[ApiController]
[Route("api/estadisticas")]
[Authorize]
public class EstadisticasController : ControllerBase
{
    private readonly IEstadisticasService _estadisticasService;

    public EstadisticasController(IEstadisticasService estadisticasService)
    {
        _estadisticasService = estadisticasService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerResumen()
    {
        var resultado = await _estadisticasService.ObtenerResumenAsync();
        return Ok(resultado);
    }
}
