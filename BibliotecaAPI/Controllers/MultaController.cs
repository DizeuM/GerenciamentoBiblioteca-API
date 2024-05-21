using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Exceptions;

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
    public async Task<IActionResult> CalculaMulta()
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
    public async Task<IActionResult> Get()
    {
        var multaResponse = await _multaService.GetMultas();
        return Ok(multaResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
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


    [HttpPost("Pagar/{id}")]
    public async Task<IActionResult> Post(int id)
    {
        try
        {
            await _multaService.PagarMulta(id);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}