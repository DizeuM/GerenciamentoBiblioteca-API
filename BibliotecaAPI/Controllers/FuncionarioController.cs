using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
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

    /// <summary>
    /// Cria um novo funcionário.
    /// </summary>
    /// <param name="funcionarioDto">Os dados do funcionário a ser criado.</param>
    /// <returns>Retorna o funcionário criado.</returns>
    /// <response code="201">Funcionário criado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadFuncionarioDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> CriaFuncionario([FromBody] CreateFuncionarioDto funcionarioDto)
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

    /// <summary>
    /// Obtém todos os funcionários.
    /// </summary>
    /// <returns>Retorna uma lista de funcionários.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadFuncionarioDto>), 200)]
    public async Task<IActionResult> ObtemFuncionarios()
    {
        var funcionariosDtoResponse = await _funcionarioService.GetAllFuncionarios();
        return Ok(funcionariosDtoResponse);
    }

    /// <summary>
    /// Obtém funcionários por atributos.
    /// </summary>
    /// <param name="searchFuncionarioDto">Os atributos para filtrar os funcionários.</param>
    /// <returns>Retorna uma lista de funcionários que correspondem aos atributos fornecidos.</returns>
    [HttpGet("Atributos/")]
    [ProducesResponseType(typeof(IEnumerable<ReadFuncionarioDto>), 200)]
    public async Task<IActionResult> ObtemFuncionarioPorAtributos([FromQuery] SearchFuncionarioDto searchFuncionarioDto)
    {
        var funcionariosDtoResponse = await _funcionarioService.SearchFuncionarioByAttributes(searchFuncionarioDto);
        return Ok(funcionariosDtoResponse);
    }

    /// <summary>
    /// Obtém um funcionário pelo ID.
    /// </summary>
    /// <param name="id">ID do funcionário.</param>
    /// <returns>Retorna o funcionário com o ID especificado.</returns>
    /// <response code="200">Funcionário encontrado.</response>
    /// <response code="404">Funcionário não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadFuncionarioDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemFuncionario(int id)
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

    /// <summary>
    /// Atualiza um funcionário.
    /// </summary>
    /// <param name="id">ID do funcionário a ser atualizado.</param>
    /// <param name="funcionarioDto">Os dados do funcionário a serem atualizados.</param>
    /// <returns>Retorna NoContent se o funcionário foi atualizado com sucesso.</returns>
    /// <response code="204">Funcionário atualizado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="404">Funcionário não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> AtualizaFuncionario(int id, [FromBody] UpdateFuncionarioDto funcionarioDto)
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