using ProyectoFinal_ChavesCamposAndres.Data;
using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None
    });
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "Este campo es obligatorio.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        _ => "Ingresa un número válido.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (_, _) => "Ingresa un valor válido.");
});
builder.Services.AddDbContext<AplicacionDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AplicacionDbContext>()
.AddErrorDescriber<ErroresIdentity>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Ingresar";
    options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
});
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseExceptionHandler("/Inicio/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");
await DatosIniciales.CargarAsync(app.Services);
app.Run();