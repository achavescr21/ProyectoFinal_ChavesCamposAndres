using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.ViewModels;

public class RegistroVM
{
    [Required(ErrorMessage = "Escribe tu nombre completo.")]
    [StringLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;
    [Required(ErrorMessage = "Escribe tu identificación.")]
    [StringLength(30, ErrorMessage = "La identificación admite hasta 30 caracteres.")]
    [Display(Name = "Identificación")]
    public string Identificacion { get; set; } = string.Empty;
    [Required(ErrorMessage = "Escribe tu correo electrónico.")]
    [EmailAddress(ErrorMessage = "Escribe un correo electrónico válido.")]
    [StringLength(256, ErrorMessage = "El correo admite hasta 256 caracteres.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Escribe una contraseña.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Clave { get; set; } = string.Empty;
    [Required(ErrorMessage = "Confirma la contraseña.")]
    [Compare(nameof(Clave), ErrorMessage = "Las contraseñas no coinciden, porfavor verifica.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarClave { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una carrera.")]
    [Display(Name = "Carrera")]
    public int CarreraId { get; set; }
}