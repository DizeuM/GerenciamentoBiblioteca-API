﻿using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Request;

public class CreateEmprestimoDto
{
    public int UsuarioId { get; set; }
    public int ExemplarId { get; set; }
}