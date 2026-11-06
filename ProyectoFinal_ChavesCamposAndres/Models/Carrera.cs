using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.Models;

public class Carrera
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Escribe el código de la carrera.")]
    [StringLength(12, ErrorMessage = "El código solo admite hasta 12 caracteres.")]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Escribe el nombre de la carrera.")]
    [StringLength(100, ErrorMessage = "El nombre de la carrera solo admite hasta 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}