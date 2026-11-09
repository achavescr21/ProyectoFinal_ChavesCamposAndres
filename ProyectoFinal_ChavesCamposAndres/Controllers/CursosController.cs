using ProyectoFinal_ChavesCamposAndres.Data;
using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal_ChavesCamposAndres.Controllers;

[Authorize(Roles = "Administrador")]
public class CursosController : Controller
{
    private readonly AplicacionDbContext _db;
    public CursosController(AplicacionDbContext db)
    {
        _db = db;
    }
    private async Task CargarCarreras(int? seleccion = null)
    {
        ViewBag.Carreras = new SelectList(
            await _db.Carreras.AsNoTracking()
                .OrderBy(x => x.Nombre).ToListAsync(),
            "Id", "Nombre", seleccion);
    }
    public async Task<IActionResult> Index()
    {
        return View(await _db.Cursos.AsNoTracking()
            .Include(x => x.Carrera)
            .OrderBy(x => x.Codigo)
            .ToListAsync());
    }
    public async Task<IActionResult> Crear()
    {
        await CargarCarreras();
        return View(new Curso { Creditos = 1 });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(
        [Bind("Codigo,Nombre,Creditos,CarreraId")] Curso curso)
    {
        curso.Codigo = (curso.Codigo ?? string.Empty)
            .Trim().ToUpperInvariant();
        curso.Nombre = (curso.Nombre ?? string.Empty).Trim();
        await CargarCarreras(curso.CarreraId);
        if (await _db.Cursos.AnyAsync(x => x.Codigo == curso.Codigo))
        {
            ModelState.AddModelError(
                nameof(curso.Codigo),
                "El código del curso ya está registrado.");
        }
        if (!await _db.Carreras.AnyAsync(x => x.Id == curso.CarreraId))
        {
            ModelState.AddModelError(
                nameof(curso.CarreraId), "Selecciona una carrera válida.");
        }
        if (!ModelState.IsValid)
        {
            return View(curso);
        }
        _db.Cursos.Add(curso);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                string.Empty,
                "No se pudo guardar el curso. Revisa sus datos.");

            return View(curso);
        }
        TempData["Exito"] = "El curso quedó registrado.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Editar(int id)
    {
        var curso = await _db.Cursos.FindAsync(id);
        if (curso is null)
        {
            return NotFound();
        }
        await CargarCarreras(curso.CarreraId);
        return View(curso);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        int id, [Bind("Id,Codigo,Nombre,Creditos,CarreraId")] Curso entrada)
    {
        if (id != entrada.Id)
        {
            return NotFound();
        }
        var curso = await _db.Cursos.FindAsync(id);
        if (curso is null)
        {
            return NotFound();
        }
        entrada.Codigo = curso.Codigo;
        await CargarCarreras(entrada.CarreraId);
        if (!await _db.Carreras.AnyAsync(x => x.Id == entrada.CarreraId))
        {
            ModelState.AddModelError(
                nameof(entrada.CarreraId), "Selecciona una carrera válida.");
        }
        if (curso.CarreraId != entrada.CarreraId
            && await _db.Matriculas.AnyAsync(x => x.CursoId == id))
        {
            ModelState.AddModelError(
                nameof(entrada.CarreraId),
                "Un curso con matrículas no puede cambiar de carrera.");
        }
        if (!ModelState.IsValid)
        {
            return View(entrada);
        }
        if (curso.CarreraId != entrada.CarreraId)
        {
            curso.DocenteId = null;
        }
        curso.Nombre = entrada.Nombre.Trim();
        curso.Creditos = entrada.Creditos;
        curso.CarreraId = entrada.CarreraId;
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
    public async Task<IActionResult> Eliminar(int id)
    {
        var curso = await _db.Cursos
            .Include(x => x.Carrera)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (curso is null)
        {
            return NotFound();
        }
        ViewBag.Bloqueada = await _db.Matriculas
            .AnyAsync(x => x.CursoId == id);
        return View(curso);
    }
    [HttpPost, ActionName("Eliminar"), ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var curso = await _db.Cursos
            .Include(x => x.Carrera)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (curso is null)
        {
            return NotFound();
        }
        if (await _db.Matriculas.AnyAsync(x => x.CursoId == id))
        {
            ViewBag.Bloqueada = true;
            ModelState.AddModelError(
                string.Empty,
                "El curso tiene matrículas asociadas y no puede eliminarse.");
            return View(curso);
        }
        _db.Cursos.Remove(curso);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ViewBag.Bloqueada = true;

            ModelState.AddModelError(
                string.Empty,
                "El curso tiene registros relacionados y no puede eliminarse.");

            return View(curso);
        }
        TempData["Exito"] = "El curso quedó eliminado.";
        return RedirectToAction(nameof(Index));
    }
}