using BibliotecaAPI.Data.Dtos.Request;
using BibliotecaAPI.Exceptions;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]

public class ExemplarController : ControllerBase
{
    private readonly IExemplarService _exemplarService;

    public ExemplarController(IExemplarService exemplarService)
    {
        _exemplarService = exemplarService;
    }

    [HttpPost]
    public async Task<IActionResult> CriaExemplar([FromBody] CreateExemplarDto exemplarDto)
    {
        try
        {
            var exemplarDtoResponse = await _exemplarService.CreateExemplar(exemplarDto);
            return Created(string.Empty, exemplarDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtemExemplares()
    {
        var exemplaresDtoResponse = await _exemplarService.GetAllExemplares();
        return Ok(exemplaresDtoResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtemExemplar(int id)
    {
        try
        {
            var exemplarDtoResponse = await _exemplarService.GetExemplarById(id);
            return Ok(exemplarDtoResponse);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizaStatusExemplar(int id)
    {
        try
        {
            await _exemplarService.UpdateExemplar(id);
            return NoContent();
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
}
