﻿using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Data.Dtos.Response;

public class ReadMultaDto
{
    public int Id { get; set; }
    public int EmprestimoId { get; set; }
    public double Valor { get; set; }
    public string DiasAtrasados { get; set; }
    public DateTime InicioMulta { get; set; }
    public DateTime? FimMulta { get; set; }
    public int Status { get; set; }
}
