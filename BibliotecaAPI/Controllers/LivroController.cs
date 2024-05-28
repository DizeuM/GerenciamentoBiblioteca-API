using BibliotecaAPI.Data.Dtos.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;

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

    [HttpPost]
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

    [HttpGet]
    public async Task<IActionResult> ObtemLivros()
    {
        var livrosDtoResponse = await _livroService.GetLivros();
        return Ok(livrosDtoResponse);
    }

    [HttpGet("Atributos/")]
    public async Task<IActionResult> ObtemLivrosPorAtributos([FromQuery] SearchLivroDto livroDto)
    {
        var livrosDtoResponse = await _livroService.SearchLivroByAttributes(livroDto);
        return Ok(livrosDtoResponse);
    }

    [HttpGet("{id}")]
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

    [HttpGet("{id}/Exemplares")]
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

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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
