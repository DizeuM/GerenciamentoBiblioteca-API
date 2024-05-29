using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IEmprestimoService _emprestimoService;
    private readonly IMultaService _multaService;

    public UsuarioController(IUsuarioService usuarioService, IEmprestimoService emprestimoService, IMultaService multaService)
    {
        _usuarioService = usuarioService;
        _emprestimoService = emprestimoService;
        _multaService = multaService;
    }

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="usuarioDto">Os dados do usuário a ser criado.</param>
    /// <returns>Retorna o usuário criado.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadUsuarioDto), 201)]
    [ProducesResponseType(typeof(string), 400)]

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


    /// <summary>
    /// Obtém todos os usuários.
    /// </summary>
    /// <returns>Retorna uma lista de usuários.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadUsuarioDto>), 200)]
    public async Task<IActionResult> ObtemUsuarios()
    {
        var usuariosDtoResponse = await _usuarioService.GetAllUsuarios();
        return Ok(usuariosDtoResponse);
    }

    /// <summary>
    /// Obtém usuários por atributos.
    /// </summary>
    /// <param name="searchUsuarioDto">Os atributos para filtrar os usuários.</param>
    /// <returns>Retorna uma lista de usuários que correspondem aos atributos fornecidos.</returns>
    [HttpGet("Atributos/")]
    [ProducesResponseType(typeof(IEnumerable<ReadUsuarioDto>), 200)]
    public async Task<IActionResult> ObtemUsuarioPorAtributos([FromQuery] SearchUsuarioDto searchUsuarioDto)
    {
        var usuariosDtoResponse = await _usuarioService.SearchUsuarioByAttributes(searchUsuarioDto);
        return Ok(usuariosDtoResponse);
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Retorna o usuário com o ID especificado.</returns>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadUsuarioDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Obtém o histórico de empréstimos de um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Retorna uma lista de empréstimos do usuário especificado.</returns>
    /// <response code="200">Histórico de empréstimos encontrado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id}/Emprestimos")]
    [ProducesResponseType(typeof(IEnumerable<ReadEmprestimoDto>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemHistoricoDeEmprestimosDoUsuario(int id)
    {
        try
        {
            var emprestimoResponse = await _emprestimoService.GetEmprestimosUsuario(id);
            return Ok(emprestimoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Obtém o histórico de multas de um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>Retorna uma lista de multas do usuário especificado.</returns>
    /// <response code="200">Histórico de multas encontrado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id}/Multas")]
    [ProducesResponseType(typeof(IEnumerable<ReadMultaDto>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemHistoricoDeMultasDoUsuario(int id)
    {
        try
        {
            var multaResponse = await _multaService.GetMultasUsuario(id);
            return Ok(multaResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Atualiza um usuário.
    /// </summary>
    /// <param name="id">ID do usuário a ser atualizado.</param>
    /// <param name="usuarioDto">Os dados do usuário a serem atualizados.</param>
    /// <returns>Retorna NoContent se o usuário foi atualizado com sucesso.</returns>
    /// <response code="204">Usuário atualizado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Exclui um usuário.
    /// </summary>
    /// <param name="id">ID do usuário a ser excluído.</param>
    /// <returns>Retorna NoContent se o usuário foi excluído com sucesso.</returns>
    /// <response code="204">Usuário excluído com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
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