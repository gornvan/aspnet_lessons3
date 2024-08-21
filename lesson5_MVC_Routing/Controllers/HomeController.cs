using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lesson5_MVC_Routing.Models;

namespace lesson5_MVC_Routing.Controllers;

public class HomeController
    (ILogger<HomeController> logger)
    : Controller
{

    public IActionResult Index()
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
