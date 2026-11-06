using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Identity;
namespace ProyectoFinal_ChavesCamposAndres.Data;

public static class DatosIniciales
{
    public static async Task CargarAsync(IServiceProvider servicios)
    {
        using var alcance = servicios.CreateScope();
        var roles = alcance.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();
        var usuarios = alcance.ServiceProvider
            .GetRequiredService<UserManager<Usuario>>();
        var configuracion = alcance.ServiceProvider
            .GetRequiredService<IConfiguration>();
        foreach (var nombre in new[] { "Administrador", "Estudiante" })
        {
            if (!await roles.RoleExistsAsync(nombre))
            {
                var resultado = await roles.CreateAsync(new IdentityRole(nombre));
                if (!resultado.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(" ", resultado.Errors.Select(x => x.Description)));
                }
            }
        }
        var correo = configuracion["Administrador:Correo"];
        var clave = configuracion["Administrador:Clave"];
        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(clave))
        {
            throw new InvalidOperationException(
                "Configura el correo y la clave del administrador en los secretos de usuario.");
        }
        var administrador = await usuarios.FindByEmailAsync(correo);
        if (administrador is null)
        {
            administrador = new Usuario
            {
                NombreCompleto = "Administrador",
                UserName = correo,
                Email = correo,
                EmailConfirmed = true
            };
            var resultado = await usuarios.CreateAsync(administrador, clave);
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(" ", resultado.Errors.Select(x => x.Description)));
            }
        }
        if (!await usuarios.IsInRoleAsync(administrador, "Administrador"))
        {
            var resultado = await usuarios.AddToRoleAsync(
                administrador, "Administrador");
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(" ", resultado.Errors.Select(x => x.Description)));
            }
        }
    }
}