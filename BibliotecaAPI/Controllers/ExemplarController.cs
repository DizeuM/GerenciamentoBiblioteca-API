using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[AllowAnonymous]
[Route("[controller]")]
[ApiController]

public class ExemplarController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriaExemplar()
    {
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> ObtemExemplares()
    {
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtemExemplar(int id)
    {
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizaStatusExemplar(int id)
    {
        return NoContent();
    }
}
