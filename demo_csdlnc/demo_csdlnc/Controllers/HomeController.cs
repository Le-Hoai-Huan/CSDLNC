using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using demo_csdlnc.Models;

namespace demo_csdlnc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string username = HttpContext.Session.GetString("Username");
        string role = HttpContext.Session.GetString("Role");

        ViewBag.Username = username;
        ViewBag.Role = role;

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
