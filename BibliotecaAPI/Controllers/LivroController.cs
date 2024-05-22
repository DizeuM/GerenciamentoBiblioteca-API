using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaAPI.Exceptions;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class LivroController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivroController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateLivroDto livroDto)
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

    [HttpGet("Atributos/")]
    public async Task<IActionResult> GetByAttributes([FromQuery] SearchLivroDto livroDto)
    {
        var livrosDtoResponse = await _livroService.SearchLivroByAttributes(livroDto);
        return Ok(livrosDtoResponse);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var livrosDtoResponse = await _livroService.GetLivros();
        return Ok(livrosDtoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateLivroDto livroDto)
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
    public async Task<IActionResult> Delete(int id)
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
