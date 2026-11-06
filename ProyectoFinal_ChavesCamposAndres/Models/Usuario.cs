using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace ProyectoFinal_ChavesCamposAndres.Models;
public class Usuario : IdentityUser
{
    [Required, StringLength(100)]
    public string NombreCompleto { get; set; } = string.Empty;
}