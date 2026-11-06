using ProyectoFinal_ChavesCamposAndres.Data;
using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal_ChavesCamposAndres.Controllers;

[Authorize(Roles = "Estudiante")]
public class EstudianteController : Controller
{
    private readonly AplicacionDbContext _db;
    private readonly UserManager<Usuario> _usuarios;
    public EstudianteController(
        AplicacionDbContext db,
        UserManager<Usuario> usuarios)
    {
        _db = db;
        _usuarios = usuarios;
    }
    public async Task<IActionResult> Index()
    {
        var id = _usuarios.GetUserId(User);
        var estudiante = await _db.Estudiantes.AsNoTracking()
            .Include(x => x.Usuario)
            .Include(x => x.Carrera)
            .FirstOrDefaultAsync(x => x.UsuarioId == id);
        if (estudiante is null)
        {
            return NotFound();
        }
        return View(estudiante);
    }
    public async Task<IActionResult> Cursos()
    {
        var id = _usuarios.GetUserId(User);
        var estudiante = await _db.Estudiantes.AsNoTracking()
            .Include(x => x.Carrera)
            .FirstOrDefaultAsync(x => x.UsuarioId == id);
        if (estudiante is null)
        {
            return NotFound();
        }
        ViewBag.Carrera = estudiante.Carrera?.Nombre;
        var cursos = await _db.Cursos.AsNoTracking()
            .Include(x => x.Docente)
            .Where(x => x.CarreraId == estudiante.CarreraId)
            .OrderBy(x => x.Codigo)
            .ToListAsync();
        return View(cursos);
    }
}