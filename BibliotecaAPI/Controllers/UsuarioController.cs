﻿using BibliotecaAPI.Data.Dtos.Request;
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
    public async Task<IActionResult> CriaUsuario([FromBody] CreateUsuarioDto usuarioDto)
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
    public async Task<IActionResult> ObtemUsuarios()
    {
        var usuariosDtoResponse = await _usuarioService.GetAllUsuarios();
        return Ok(usuariosDtoResponse);
    }

    [HttpGet("Atributos/")]
    public async Task<IActionResult> ObtemUsuarioPorAtributos()
    {
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtemUsuario(int id)
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

    [HttpGet("{id}/Emprestimos")]
    public async Task<IActionResult> ObtemHistoricoDeEmprestimosDoUsuario(int id)
    {
        return NoContent();
    }

    [HttpGet("{id}/Multas")]
    public async Task<IActionResult> ObtemHistoricoDeMultasDoUsuario(int id)
    {
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizaUsuario(int id, [FromBody] UpdateUsuarioDto usuarioDto)
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
    public async Task<IActionResult> ExcluiUsuario(int id)
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