using BibliotecaAPI.Data.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class FuncionarioController : ControllerBase
{
    private readonly IFuncionarioService _funcionarioService;

    public FuncionarioController(IFuncionarioService funcionarioService)
    {
        _funcionarioService = funcionarioService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateFuncionarioDto funcionarioDto)
    {
        var funcionarioDtoResponse = await _funcionarioService.CreateFuncionario(funcionarioDto);
        if (funcionarioDtoResponse == null) { return BadRequest(); }
        return Created(string.Empty, funcionarioDtoResponse);

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var funcionariosDto = await _funcionarioService.GetAllFuncionarios();
        return Ok(funcionariosDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var funcionarioDto = await _funcionarioService.GetFuncionarioDto(id);
        if (funcionarioDto == null){return NotFound();}
        return Ok(funcionarioDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateFuncionarioDto funcionarioDto)
    {
        await _funcionarioService.UpdateFuncionario(id, funcionarioDto);
        return NoContent();
    }
}