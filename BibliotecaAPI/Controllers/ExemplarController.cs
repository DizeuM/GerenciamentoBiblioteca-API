using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]

public class ExemplarController : ControllerBase
{
    private readonly IExemplarService _exemplarService;

    public ExemplarController(IExemplarService exemplarService)
    {
        _exemplarService = exemplarService;
    }

    /// <summary>
    /// Cria um novo exemplar.
    /// </summary>
    /// <param name="exemplarDto">Os dados do exemplar a ser criado.</param>
    /// <returns>Retorna o exemplar criado.</returns>
    /// <response code="201">Exemplar criado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadExemplarDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> CriaExemplar([FromBody] CreateExemplarDto exemplarDto)
    {
        try
        {
            var exemplarDtoResponse = await _exemplarService.CreateExemplar(exemplarDto);
            return Created(string.Empty, exemplarDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Obtém todos os exemplares.
    /// </summary>
    /// <returns>Retorna uma lista de exemplares.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadExemplarDto>), 200)]
    public async Task<IActionResult> ObtemExemplares()
    {
        var exemplaresDtoResponse = await _exemplarService.GetAllExemplares();
        return Ok(exemplaresDtoResponse);
    }

    /// <summary>
    /// Obtém um exemplar pelo ID.
    /// </summary>
    /// <param name="id">ID do exemplar.</param>
    /// <returns>Retorna o exemplar com o ID especificado.</returns>
    /// <response code="200">Exemplar encontrado.</response>
    /// <response code="404">Exemplar não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadExemplarDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemExemplar(int id)
    {
        try
        {
            var exemplarDtoResponse = await _exemplarService.GetExemplarById(id);
            return Ok(exemplarDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza o status de um exemplar.
    /// </summary>
    /// <param name="id">ID do exemplar a ser atualizado.</param>
    /// <returns>Retorna NoContent se o status do exemplar foi atualizado com sucesso.</returns>
    /// <response code="204">Status do exemplar atualizado com sucesso.</response>
    /// <response code="404">Exemplar não encontrado.</response>
    /// <response code="400">Exemplar está em uso.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> AtualizaStatusExemplar(int id)
    {
        try
        {
            await _exemplarService.UpdateExemplar(id);
            return NoContent();
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
