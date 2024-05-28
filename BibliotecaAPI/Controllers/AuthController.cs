using BibliotecaAPI.Data.Dtos.Request;
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

    [HttpPost]
    [AllowAnonymous]
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
