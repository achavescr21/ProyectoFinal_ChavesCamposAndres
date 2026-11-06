using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.Models;

public class Docente
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string NombreCompleto { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string Identificacion { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(256)]
    public string Correo { get; set; } = string.Empty;
    public int? CarreraId { get; set; }
    public Carrera? Carrera { get; set; }
}