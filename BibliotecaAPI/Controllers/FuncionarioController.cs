using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Exceptions;
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
        try
        {
            var funcionarioDtoResponse = await _funcionarioService.CreateFuncionario(funcionarioDto);
            return Created(string.Empty, funcionarioDtoResponse);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var funcionariosDtoResponse = await _funcionarioService.GetAllFuncionarios();
        return Ok(funcionariosDtoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var funcionarioDtoResponse = await _funcionarioService.GetFuncionarioById(id);
            return Ok(funcionarioDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateFuncionarioDto funcionarioDto)
    {
        try
        {
            await _funcionarioService.UpdateFuncionario(id, funcionarioDto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}