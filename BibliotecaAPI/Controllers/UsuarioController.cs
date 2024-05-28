using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Exceptions;
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
        try
        {
            var usuarioDtoResponse = await _usuarioService.CreateUsuario(usuarioDto);
            return Created(string.Empty, usuarioDtoResponse);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuariosDtoResponse = await _usuarioService.GetAllUsuarios();
        return Ok(usuariosDtoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var usuarioDtoResponse = await _usuarioService.GetUsuarioById(id);
            return Ok(usuarioDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateUsuarioDto usuarioDto)
    {
        try
        {
            await _usuarioService.UpdateUsuario(id, usuarioDto);
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
            await _usuarioService.DeleteUsuario(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}