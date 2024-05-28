using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class MultaController : ControllerBase
{
    private readonly IMultaService _multaService;

    public MultaController(IMultaService multaService)
    {
        _multaService = multaService;
    }

    [HttpPost("CalcularMultas/")]
    public async Task<IActionResult> CriaEAtualizaMultas()
    {
        try
        {
            await _multaService.CreateAndUpdateMultas();
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtemMultas()
    {
        var multaResponse = await _multaService.GetMultas();
        return Ok(multaResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtemMulta(int id)
    {
        try
        {
            var multaResponse = await _multaService.GetMultaById(id);
            return Ok(multaResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{id}/Pagar")]
    public async Task<IActionResult> a(int id)
    {
        try
        {
            await _multaService.PayMulta(id);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}