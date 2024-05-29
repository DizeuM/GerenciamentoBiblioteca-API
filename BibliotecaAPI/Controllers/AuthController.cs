using BibliotecaAPI.Dtos.Request;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Autentica um usuário e gera um token JWT.
    /// </summary>
    /// <param name="login">Dados de login do usuário.</param>
    /// <returns>Retorna o token JWT se a autenticação for bem-sucedida.</returns>
    /// <response code="200">Autenticação bem-sucedida.</response>
    /// <response code="400">Requisição inválida.</response>
    /// <response code="401">CPF ou Senha incorretos.</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 401)]
    public async Task<IActionResult> Auth([FromBody] AuthDto login)
    {
        try
        {
            var funcionarioId = await _authService.AuthenticateUser(login);
            var token = await _authService.GenerateToken(funcionarioId);
            return Ok(token);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}
