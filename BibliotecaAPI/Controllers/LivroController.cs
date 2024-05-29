using BibliotecaAPI.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Dtos.Response;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class LivroController : ControllerBase
{
    private readonly ILivroService _livroService;
    private readonly IExemplarService _exemplarService;

    public LivroController(ILivroService livroService, IExemplarService exemplarService)
    {
        _livroService = livroService;
        _exemplarService = exemplarService;
    }

    /// <summary>
    /// Cria um novo livro.
    /// </summary>
    /// <param name="livroDto">Os dados do livro a ser criado.</param>
    /// <returns>Retorna o livro criado.</returns>
    /// <response code="201">Livro criado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadLivroDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> CriaLivro([FromBody] CreateLivroDto livroDto)
    {
        try
        {
            var livroDtoResponse = await _livroService.CreateLivro(livroDto);
            return Created(string.Empty, livroDtoResponse);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtém todos os livros.
    /// </summary>
    /// <returns>Retorna uma lista de livros.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadLivroDto>), 200)]
    public async Task<IActionResult> ObtemLivros()
    {
        var livrosDtoResponse = await _livroService.GetLivros();
        return Ok(livrosDtoResponse);
    }

    /// <summary>
    /// Obtém livros por atributos.
    /// </summary>
    /// <param name="livroDto">Os atributos para filtrar os livros.</param>
    /// <returns>Retorna uma lista de livros que correspondem aos atributos fornecidos.</returns>
    [HttpGet("Atributos/")]
    [ProducesResponseType(typeof(IEnumerable<ReadLivroDto>), 200)]
    public async Task<IActionResult> ObtemLivrosPorAtributos([FromQuery] SearchLivroDto livroDto)
    {
        var livrosDtoResponse = await _livroService.SearchLivroByAttributes(livroDto);
        return Ok(livrosDtoResponse);
    }

    /// <summary>
    /// Obtém um livro pelo ID.
    /// </summary>
    /// <param name="id">ID do livro.</param>
    /// <returns>Retorna o livro com o ID especificado.</returns>
    /// <response code="200">Livro encontrado.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadLivroDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemLivro(int id)
    {
        try
        {
            var livroDtoResponse = await _livroService.GetLivroById(id);
            return Ok(livroDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Obtém os exemplares pelo ID do livro.
    /// </summary>
    /// <param name="id">ID do livro.</param>
    /// <returns>Retorna uma lista de exemplares do livro especificado.</returns>
    /// <response code="200">Exemplares encontrados.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpGet("{id}/Exemplares")]
    [ProducesResponseType(typeof(IEnumerable<ReadExemplarDto>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemExemplaresDoLivro(int id)
    {
        try
        {
            var exemplarResponse = await _exemplarService.GetLivroExemplares(id);
            return Ok(exemplarResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza um livro.
    /// </summary>
    /// <param name="id">ID do livro a ser atualizado.</param>
    /// <param name="livroDto">Os dados do livro a serem atualizados.</param>
    /// <returns>Retorna NoContent se o livro foi atualizado com sucesso.</returns>
    /// <response code="204">Livro atualizado com sucesso.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AtualizaLivro(int id, [FromBody] UpdateLivroDto livroDto)
    {
        try
        {
            await _livroService.UpdateLivro(id, livroDto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Exclui um livro.
    /// </summary>
    /// <param name="id">ID do livro a ser excluído.</param>
    /// <returns>Retorna NoContent se o livro foi excluído com sucesso.</returns>
    /// <response code="204">Livro excluído com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="404">Livro não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ExcluiLivro(int id)
    {
        try
        {
            await _livroService.DeleteLivro(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
