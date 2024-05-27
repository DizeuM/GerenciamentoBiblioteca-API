using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class RenovacaoController : ControllerBase
{
    private readonly IRenovacaoService _renovacaoService;

    public RenovacaoController(IRenovacaoService renovacaoService)
    {
        _renovacaoService = renovacaoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var renovacaoResponse = await _renovacaoService.GetRenovacoes();
        return Ok(renovacaoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var renovacaoResponse = await _renovacaoService.GetRenovacaoById(id);
            return Ok(renovacaoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPost("{emprestimoId}")]
    public async Task<IActionResult> Post(int emprestimoId)
    {
        try
        {
            var renovacaoResponse = await _renovacaoService.CreateRenovacao(emprestimoId);
            return Ok(renovacaoResponse);
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