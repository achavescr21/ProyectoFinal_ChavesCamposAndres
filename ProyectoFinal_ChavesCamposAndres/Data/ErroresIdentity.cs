using Microsoft.AspNetCore.Identity;
namespace ProyectoFinal_ChavesCamposAndres.Data;

public class ErroresIdentity : IdentityErrorDescriber
{
    private static IdentityError Crear(string codigo, string texto) =>
        new() { Code = codigo, Description = texto };
    public override IdentityError DuplicateEmail(string email) =>
        Crear(nameof(DuplicateEmail), "El correo ya está registrado.");
    public override IdentityError DuplicateUserName(string userName) =>
        Crear(nameof(DuplicateUserName), "El correo ya está registrado.");
    public override IdentityError InvalidUserName(string? userName) =>
        Crear(nameof(InvalidUserName), "El correo contiene caracteres no permitidos.");
    public override IdentityError InvalidEmail(string? email) =>
        Crear(nameof(InvalidEmail), "Escribe un correo válido.");
    public override IdentityError PasswordTooShort(int length) =>
        Crear(nameof(PasswordTooShort),
            $"La contraseña debe tener al menos {length} caracteres.");
    public override IdentityError PasswordRequiresDigit() =>
        Crear(nameof(PasswordRequiresDigit),
            "La contraseña debe incluir un número.");
    public override IdentityError PasswordRequiresLower() =>
        Crear(nameof(PasswordRequiresLower),
            "La contraseña debe incluir una minúscula.");
    public override IdentityError PasswordRequiresUpper() =>
        Crear(nameof(PasswordRequiresUpper),
            "La contraseña debe incluir una mayúscula.");
    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        Crear(nameof(PasswordRequiresNonAlphanumeric),
            "La contraseña debe incluir un símbolo.");
}