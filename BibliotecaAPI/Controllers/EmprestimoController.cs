using BibliotecaAPI.Data.Dtos.Request;
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
    private readonly IRenovacaoService _renovacaoService;

    public EmprestimoController(IEmprestimoService emprestimoService, IRenovacaoService renovacaoService)
    {
        _emprestimoService = emprestimoService;
        _renovacaoService = renovacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriaEmprestimo([FromBody] CreateEmprestimoDto emprestimoDto)
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
    public async Task<IActionResult> ObtemEmprestimos()
    {
        var emprestimosDtoResponse = await _emprestimoService.GetEmprestimos();
        return Ok(emprestimosDtoResponse);
    }

    [HttpGet("Renovacoes/")]
    public async Task<IActionResult> ObtemRenovacoesDosEmprestimos()
    {
        var renovacaoResponse = await _renovacaoService.GetRenovacoes();
        return Ok(renovacaoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtemEmprestimo(int id)
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

    [HttpGet("{id}/Multas")]
    public async Task<IActionResult> ObtemMultasDoEmprestimo(int id)
    {
        return NoContent();
    }

    [HttpPost("{id}/Devolver")]
    public async Task<IActionResult> DevolveEmprestimo(int id)
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

    [HttpPost("{id}/Renovar")]
    public async Task<IActionResult> RenovaEmprestimo(int id)
    {
        try
        {
            var renovacaoResponse = await _renovacaoService.CreateRenovacao(id);
            return Ok(renovacaoResponse);
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

    [HttpGet("{id}/Renovacoes")]
    public async Task<IActionResult> ObtemRenovacoesDoEmprestimo(int id)
    {
        return NoContent();
    }

    [HttpPut("AtualizarEmprestimosAtrasados/")]
    public async Task<IActionResult> AtualizaEmprestimosAtrasados()
    {
        await _emprestimoService.UpdateEmprestimosAtrasados();
        return NoContent();
    }
}