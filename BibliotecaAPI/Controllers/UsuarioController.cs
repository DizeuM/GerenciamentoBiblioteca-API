using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUsuarioDto usuarioDto)
    { 
        var usuarioDtoResponse = await _usuarioService.CreateUsuario(usuarioDto);
        if (usuarioDtoResponse == null) { return BadRequest(); }
        return Created(string.Empty, usuarioDtoResponse);

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuariosDto = await _usuarioService.GetAllUsuarios();
        return Ok(usuariosDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var usuariosDto = await _usuarioService.GetUsuarioById(id);
        if (usuariosDto == null) { return NotFound(); }
        return Ok(usuariosDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateUsuarioDto usuarioDto)
    {
        await _usuarioService.UpdateUsuario(id, usuarioDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _usuarioService.DeleteUsuario(id);
        return NoContent();
    }
}