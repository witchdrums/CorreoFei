using CorreoFei.Models;
using CorreoFei.Services.Email;
using CorreoFei.Services.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CorreoFei.Controllers;
public class HomeController : Controller
{
    private readonly IEmail _email;
    private readonly IErrorLog _errorLog;

    public HomeController(IEmail email, IErrorLog errorLog)
    {
        _email = email;
        _errorLog = errorLog;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync(ContactoViewModel contacto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _email.EnviarCorreoAsync("Correo electronico desde FEI", contacto.Correo, null, null, CuerpoCorreo(contacto.Nombre));

                return RedirectToAction(nameof(Success));
            }
            catch (Exception ex)
            {
                await _errorLog.ErrorLogAsync(ex.Message);
            }
        }

        ModelState.AddModelError("", "No ha sido posible enviar el correo. Intentelo nuevamente.");
        return View();
    }

    public string CuerpoCorreo(string nombre)
    {
        return 
            $"<h1>hola {nombre} :D</h1>" +
            $"<br>" +
            $"<p>...bye!</p>";
    }

    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
