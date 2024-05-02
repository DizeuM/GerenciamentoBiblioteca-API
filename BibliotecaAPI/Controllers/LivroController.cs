using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        var response = await _livroService.CreateLivro(livroDto);
        return Created(string.Empty, response);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var livros = await _livroService.GetLivros();
        return Ok(livros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var livro = await _livroService.GetLivroById(id);
        if (livro == null) return NotFound();

        return Ok(livro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateLivroDto livroDto)
    {
        await _livroService.UpdateLivro(id, livroDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _livroService.DeleteLivro(id);
        return NoContent();
    }
}
