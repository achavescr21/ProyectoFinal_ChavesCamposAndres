using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ProyectoFinal_ChavesCamposAndres.Controllers;

[Authorize]
public class InicioController : Controller
{
    public IActionResult Index()
    {
        if (User.IsInRole("Administrador"))
        {
            return View("Administrador");
        }
        return RedirectToAction("Index", "Estudiante");
    }
    [AllowAnonymous]
    public IActionResult Error() => View();
}