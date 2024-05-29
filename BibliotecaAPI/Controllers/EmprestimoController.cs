using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Dtos.Response;
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
    private readonly IMultaService _multaService;

    public EmprestimoController(IEmprestimoService emprestimoService, IRenovacaoService renovacaoService, IMultaService multaService)
    {
        _emprestimoService = emprestimoService;
        _renovacaoService = renovacaoService;
        _multaService = multaService;
    }

    /// <summary>
    /// Cria um novo empréstimo.
    /// </summary>
    /// <param name="emprestimoDto">Os dados do empréstimo a ser criado.</param>
    /// <returns>Retorna o empréstimo criado.</returns>
    /// <response code="201">Empréstimo criado com sucesso.</response>
    /// <response code="400">Requisição inválida ou usuário com multas pendentes ou limite de empréstimos atingido.</response>
    /// <response code="404">Usuário ou exemplar não encontrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReadEmprestimoDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Obtém todos os empréstimos.
    /// </summary>
    /// <returns>Lista de empréstimos.</returns>
    /// <response code="200">Retorna a lista de empréstimos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadEmprestimoDto>), 200)]
    public async Task<IActionResult> ObtemEmprestimos()
    {
        var emprestimosDtoResponse = await _emprestimoService.GetEmprestimos();
        return Ok(emprestimosDtoResponse);
    }

    /// <summary>
    /// Obtém todas as renovações dos empréstimos.
    /// </summary>
    /// <returns>Lista de renovações.</returns>
    /// <response code="200">Retorna a lista de renovações.</response>
    [HttpGet("Renovacoes/")]
    [ProducesResponseType(typeof(IEnumerable<ReadRenovacaoDto>), 200)]
    public async Task<IActionResult> ObtemRenovacoesDosEmprestimos()
    {
        var renovacaoResponse = await _renovacaoService.GetRenovacoes();
        return Ok(renovacaoResponse);
    }

    /// <summary>
    /// Obtém um empréstimo pelo ID.
    /// </summary>
    /// <param name="id">ID do empréstimo.</param>
    /// <returns>O empréstimo solicitado.</returns>
    /// <response code="200">Retorna o empréstimo solicitado.</response>
    /// <response code="404">Empréstimo não encontrado.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadEmprestimoDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Obtém a multa de um empréstimo pelo ID.
    /// </summary>
    /// <param name="id">ID do empréstimo.</param>
    /// <returns>Multa do empréstimo.</returns>
    /// <response code="200">Retorna multa.</response>
    /// <response code="404">Empréstimo não encontrado ou emprestimo não possui multa.</response>
    [HttpGet("{id}/Multas")]
    [ProducesResponseType(typeof(IEnumerable<ReadMultaDto>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> ObtemMultasDoEmprestimo(int id)
    {
        try
        {
            var multaResponse = await _multaService.GetMultaByEmprestimo(id);
            return Ok(multaResponse);
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

    /// <summary>
    /// Devolve um empréstimo.
    /// </summary>
    /// <param name="id">ID do empréstimo.</param>
    /// <response code="204">Empréstimo devolvido com sucesso.</response>
    /// <response code="200">Empréstimo devolvido com sucesso mas tem multa pendente</response>
    /// <response code="404">Empréstimo não encontrado.</response>
    [HttpPost("{id}/Devolver")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Renova um empréstimo.
    /// </summary>
    /// <param name="id">ID do empréstimo.</param>
    /// <returns>Retorna a renovação criada.</returns>
    /// <response code="200">Renovação criada com sucesso.</response>
    /// <response code="400">Requisição inválida ou emprestimo atingiu o limite de renovações.</response>
    /// <response code="404">Empréstimo não encontrado.</response>
    [HttpPost("{id}/Renovar")]
    [ProducesResponseType(typeof(ReadRenovacaoDto), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
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

    /// <summary>
    /// Obtém as renovações de um empréstimo pelo ID.
    /// </summary>
    /// <param name="id">ID do empréstimo.</param>
    /// <returns>Lista de renovações do empréstimo.</returns>
    /// <response code="200">Retorna a lista de renovações.</response>
    [HttpGet("{id}/Renovacoes")]
    [ProducesResponseType(typeof(IEnumerable<ReadRenovacaoDto>), 200)]
    public async Task<IActionResult> ObtemRenovacoesDoEmprestimo(int id)
    {
        var renovacaoResponse = await _renovacaoService.GetRenovacoesByEmprestimo(id);
        return Ok(renovacaoResponse);
    }

    /// <summary>
    /// Atualiza os status de empréstimos atrasados.
    /// </summary>
    /// <response code="204">Status de empréstimos atrasados atualizados com sucesso.</response>
    [HttpPut("AtualizarEmprestimosAtrasados/")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> AtualizaEmprestimosAtrasados()
    {
        await _emprestimoService.UpdateEmprestimosAtrasados();
        return NoContent();
    }
}