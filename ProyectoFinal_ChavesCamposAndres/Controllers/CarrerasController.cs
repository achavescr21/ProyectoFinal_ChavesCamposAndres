using ProyectoFinal_ChavesCamposAndres.Data;
using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal_ChavesCamposAndres.Controllers;

[Authorize]
public class CarrerasController : Controller
{
    private readonly AplicacionDbContext _db;
    public CarrerasController(AplicacionDbContext db)
    {
        _db = db;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _db.Carreras.AsNoTracking()
            .OrderBy(x => x.Nombre).ToListAsync());
    }
    [Authorize(Roles = "Administrador")]
    public IActionResult Crear() => View(new Carrera());
    [HttpPost, Authorize(Roles = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        [Bind("Codigo,Nombre")] Carrera carrera)
    {
        carrera.Codigo = (carrera.Codigo ?? string.Empty)
            .Trim().ToUpperInvariant();
        carrera.Nombre = (carrera.Nombre ?? string.Empty).Trim();
        if (await _db.Carreras.AnyAsync(x => x.Codigo == carrera.Codigo))
        {
            ModelState.AddModelError(
                nameof(carrera.Codigo),
                "El código de la carrera ya está registrado.");
        }
        if (!ModelState.IsValid)
        {
            return View(carrera);
        }
        _db.Carreras.Add(carrera);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "No se pudo guardar la carrera. Revisa el código.");

            return View(carrera);
        }
        TempData["Exito"] = "La carrera quedó registrada.";
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id)
    {
        var carrera = await _db.Carreras.FindAsync(id);
        return carrera is null ? NotFound() : View(carrera);
    }
    [HttpPost, Authorize(Roles = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id, [Bind("Id,Codigo,Nombre")] Carrera entrada)
    {
        if (id != entrada.Id)
        {
            return NotFound();
        }
        var carrera = await _db.Carreras.FindAsync(id);
        if (carrera is null)
        {
            return NotFound();
        }
        entrada.Codigo = carrera.Codigo;

        if (!ModelState.IsValid)
        {
            return View(entrada);
        }
        carrera.Nombre = entrada.Nombre.Trim();
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty, "No se pudieron guardar los cambios.");

            return View(entrada);
        }
        TempData["Exito"] = "Los cambios quedaron guardados.";
        return RedirectToAction(nameof(Index));
    }
    private async Task<bool> TieneRelaciones(int id)
    {
        return await _db.Estudiantes.AnyAsync(x => x.CarreraId == id)
            || await _db.Cursos.AnyAsync(x => x.CarreraId == id)
            || await _db.Docentes.AnyAsync(x => x.CarreraId == id);
    }
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var carrera = await _db.Carreras.FindAsync(id);
        if (carrera is null)
        {
            return NotFound();
        }
        ViewBag.Bloqueada = await TieneRelaciones(id);
        return View(carrera);
    }
    [HttpPost, ActionName("Eliminar")]
    [Authorize(Roles = "Administrador"), ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var carrera = await _db.Carreras.FindAsync(id);
        if (carrera is null)
        {
            return NotFound();
        }
        if (await TieneRelaciones(id))
        {
            ViewBag.Bloqueada = true;
            ModelState.AddModelError(
                string.Empty,
                "La carrera tiene estudiantes, cursos o docentes asociados y no puede eliminarse.");
            return View(carrera);
        }
        _db.Carreras.Remove(carrera);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ViewBag.Bloqueada = true;
            ModelState.AddModelError(
                string.Empty,
                "La carrera tiene registros relacionados y no puede eliminarse.");
            return View(carrera);
        }
        TempData["Exito"] = "La carrera quedó eliminada.";
        return RedirectToAction(nameof(Index));
    }
}