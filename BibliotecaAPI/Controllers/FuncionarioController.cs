using BibliotecaAPI.Data.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

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
    public IActionResult Post([FromBody] CreateFuncionarioDto funcionarioDto)
    {
        var funcionario = _funcionarioService.CreateFuncionario(funcionarioDto);
        return Created(string.Empty, _funcionarioService.GetFuncionarioDto(funcionario.Id));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var funcionariosDto = _funcionarioService.GetAllFuncionarios();
        return Ok(funcionariosDto);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var funcionarioDto = _funcionarioService.GetFuncionarioDto(id);
        if (funcionarioDto == null){return NotFound();}
        return Ok(funcionarioDto);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdateFuncionarioDto funcionarioDto)
    {
        _funcionarioService.UpdateFuncionario(id, funcionarioDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _funcionarioService.DeleteFuncionario(id);
        return NoContent();
    }
}