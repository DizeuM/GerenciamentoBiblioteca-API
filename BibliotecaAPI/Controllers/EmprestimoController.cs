﻿using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateEmprestimoDto emprestimoDto)
    {
        try
        {
            var dadosTokenFuncionario = HttpContext.User.Identity as ClaimsIdentity;
            int funcionarioId = Int32.Parse(dadosTokenFuncionario.FindFirst("FuncionarioId").Value);

            var emprestimoDtoResponse = await _emprestimoService.CreateEmprestimo(emprestimoDto, funcionarioId);
            return Created(string.Empty, emprestimoDtoResponse);
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

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var emprestimosDtoResponse = await _emprestimoService.GetEmprestimos();
        return Ok(emprestimosDtoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var emprestimoDtoResponse = await _emprestimoService.GetEmprestimoById(id);
            return Ok(emprestimoDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("Devolver/{id}")]   
    public async Task<IActionResult> Post(int id)
    {
        try
        {
            await _emprestimoService.ReturnEmprestimo(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPut("AtualizarStatus/")]
    public async Task<IActionResult> Put()
    {
        await _emprestimoService.UpdateEmprestimosAtrasados();
        return NoContent();
    }

}