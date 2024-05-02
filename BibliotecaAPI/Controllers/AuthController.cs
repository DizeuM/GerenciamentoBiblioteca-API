using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Auth([FromBody] AuthDto login)
    {
        var token = _authService.GenerateToken(login);
        if (token == null || token == string.Empty)
        {
            return BadRequest(new { message = "CPF ou Senha incorretos" });
        }
        return Ok(token);
    }
}
