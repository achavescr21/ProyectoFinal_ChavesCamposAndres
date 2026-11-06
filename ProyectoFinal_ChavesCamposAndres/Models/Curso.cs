using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.Models;

public class Curso
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Escribe el código del curso.")]
    [StringLength(12, ErrorMessage = "El código solo admite hasta 12 caracteres.")]
    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Escribe el nombre del curso.")]
    [StringLength(100, ErrorMessage = "El nombre solo admite hasta 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Los créditos deben ser mayores que cero.")]
    [Display(Name = "Créditos")]
    public int Creditos { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una carrera.")]
    [Display(Name = "Carrera")]
    public int CarreraId { get; set; }
    public Carrera? Carrera { get; set; }
    public int? DocenteId { get; set; }
    public Docente? Docente { get; set; }
}