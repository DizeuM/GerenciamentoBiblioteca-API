using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Services;
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
        var dadosTokenFuncionario = HttpContext.User.Identity as ClaimsIdentity;
        int funcionarioId = Int32.Parse(dadosTokenFuncionario.FindFirst("FuncionarioId").Value);

        var emprestimoDtoResponse = await _emprestimoService.CreateEmprestimo(emprestimoDto, funcionarioId);
        if (emprestimoDtoResponse == null) { return BadRequest(); }

        return Created(string.Empty, emprestimoDtoResponse);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var emprestimosDtos = await _emprestimoService.GetEmprestimos();
        return Ok(emprestimosDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var emprestimoDto = await _emprestimoService.GetEmprestimoById(id);
        if (emprestimoDto == null) { return NotFound(); }
        return Ok(emprestimoDto);
    }

    [HttpPut("{id}")]   
    public async Task<IActionResult> Put(int id)
    {
        await _emprestimoService.UpdateEmprestimo(id);
        return NoContent();
    }

}