using ProyectoFinal_ChavesCamposAndres.Data;
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
        var carreras = await _db.Carreras.AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync();
        return View(carreras);
    }
}