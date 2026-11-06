using ProyectoFinal_ChavesCamposAndres.Data;
using ProyectoFinal_ChavesCamposAndres.Models;
using ProyectoFinal_ChavesCamposAndres.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal_ChavesCamposAndres.Controllers;

public class CuentaController : Controller
{
    private readonly AplicacionDbContext _db;
    private readonly UserManager<Usuario> _usuarios;
    private readonly SignInManager<Usuario> _acceso;
    public CuentaController(
        AplicacionDbContext db,
        UserManager<Usuario> usuarios,
        SignInManager<Usuario> acceso)
    {
        _db = db;
        _usuarios = usuarios;
        _acceso = acceso;
    }
    private async Task CargarCarreras(int? seleccion = null)
    {
        ViewBag.Carreras = new SelectList(
            await _db.Carreras.AsNoTracking()
                .OrderBy(x => x.Nombre).ToListAsync(),
            "Id", "Nombre", seleccion);
    }
    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> Registro()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Inicio");
        }
        await CargarCarreras();
        return View(new RegistroVM());
    }
    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroVM modelo)
    {
        await CargarCarreras(modelo.CarreraId);
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }
        modelo.Correo = modelo.Correo.Trim();
        modelo.Identificacion = modelo.Identificacion.Trim();
        if (await _usuarios.FindByEmailAsync(modelo.Correo) is not null)
        {
            ModelState.AddModelError(
                nameof(modelo.Correo), "El correo ya está registrado.");
        }
        if (await _db.Estudiantes.AnyAsync(
            x => x.Identificacion == modelo.Identificacion))
        {
            ModelState.AddModelError(
                nameof(modelo.Identificacion),
                "La identificación ya está registrada.");
        }
        if (!await _db.Carreras.AnyAsync(x => x.Id == modelo.CarreraId))
        {
            ModelState.AddModelError(
                nameof(modelo.CarreraId), "Selecciona una carrera válida.");
        }
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }
        await using var transaccion = await _db.Database.BeginTransactionAsync();
        var usuario = new Usuario
        {
            NombreCompleto = modelo.NombreCompleto.Trim(),
            UserName = modelo.Correo,
            Email = modelo.Correo
        };
        var resultado = await _usuarios.CreateAsync(usuario, modelo.Clave);
        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(
                    nameof(modelo.Clave), error.Description);
            }
            return View(modelo);
        }
        var rol = await _usuarios.AddToRoleAsync(usuario, "Estudiante");
        if (!rol.Succeeded)
        {
            ModelState.AddModelError(
                string.Empty, "No fue posible completar el registro.");
            return View(modelo);
        }
        _db.Estudiantes.Add(new Estudiante
        {
            UsuarioId = usuario.Id,
            Identificacion = modelo.Identificacion,
            CarreraId = modelo.CarreraId
        });
        try
        {
            await _db.SaveChangesAsync();
            await transaccion.CommitAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "No se pudo registrar la cuenta. Revisa el correo y la identificación.");

            return View(modelo);
        }
        await _acceso.SignInAsync(usuario, false);
        return RedirectToAction("Index", "Estudiante");
    }
    [HttpGet, AllowAnonymous]
    public IActionResult Ingresar(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Inicio");
        }
        return View(new IngresoVM { ReturnUrl = returnUrl });
    }
    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
    public async Task<IActionResult> Ingresar(IngresoVM modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }
        var usuario = await _usuarios.FindByEmailAsync(modelo.Correo.Trim());
        if (usuario is not null)
        {
            var resultado = await _acceso.PasswordSignInAsync(
                usuario, modelo.Clave, false, true);
            if (resultado.Succeeded)
            {
                if (Url.IsLocalUrl(modelo.ReturnUrl))
                {
                    return LocalRedirect(modelo.ReturnUrl!);
                }
                return RedirectToAction("Index", "Inicio");
            }
        }
        ModelState.AddModelError(
            string.Empty, "El correo o la contraseña son incorrectos.");
        return View(modelo);
    }
    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Salir()
    {
        await _acceso.SignOutAsync();
        return RedirectToAction(nameof(Ingresar));
    }
    [HttpGet, AllowAnonymous]
    public IActionResult AccesoDenegado() => View();
}