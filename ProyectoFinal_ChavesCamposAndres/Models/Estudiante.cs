using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.Models;

public class Estudiante
{
    public int Id { get; set; }
    [Required, StringLength(30)]
    public string Identificacion { get; set; } = string.Empty;
    [Required]
    public string UsuarioId { get; set; } = string.Empty;
    public Usuario? Usuario { get; set; }
    public int CarreraId { get; set; }
    public Carrera? Carrera { get; set; }
}