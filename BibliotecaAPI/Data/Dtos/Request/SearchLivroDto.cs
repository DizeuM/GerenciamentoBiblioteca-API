﻿using BibliotecaAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class SearchLivroDto
{
    public string? Nome { get; set; }
    public string? Autor { get; set; }
    public string? Editora { get; set; }
    public string? Edicao { get; set; }
    public string? Genero { get; set; }
    public LivroStatus? Status { get; set; }
}
