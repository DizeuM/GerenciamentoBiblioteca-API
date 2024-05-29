using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Dtos.Response;

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

    /// <summary>
    /// Cria e atualiza multas de empréstimos atrasados.
    /// </summary>
    /// <returns>Retorna NoContent se a operação foi bem-sucedida.</returns>
    /// <response code="204">Multas criadas e atualizadas com sucesso.</response>
    [HttpPost("CalcularMultas/")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CriaEAtualizaMultas()
    {
        await _multaService.CreateAndUpdateMultas();
        return NoContent();
    }

    /// <summary>
    /// Obtém todas as multas.
    /// </summary>
    /// <returns>Retorna uma lista de multas.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadMultaDto>), 200)]
    public async Task<IActionResult> ObtemMultas()
    {
        var multaResponse = await _multaService.GetMultas();
        return Ok(multaResponse);
    }

    /// <summary>
    /// Obtém uma multa pelo ID.
    /// </summary>
    /// <param name="id">ID da multa.</param>
    /// <returns>Retorna a multa com o ID especificado.</returns>
    /// <response code="200">Multa encontrada.</response>
    /// <response code="404">Multa não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadMultaDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
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


    /// <summary>
    /// Paga uma multa pelo ID.
    /// </summary>
    /// <param name="id">ID da multa a ser paga.</param>
    /// <returns>Retorna Ok se a operação foi bem-sucedida.</returns>
    /// <response code="200">Multa paga com sucesso.</response>
    /// <response code="404">Multa não encontrada.</response>
    /// <response code="400">Empréstimo em aberto.</response>
    [HttpPost("{id}/Pagar")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 400)]
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