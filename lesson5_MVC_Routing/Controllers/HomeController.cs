using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lesson5_MVC_Routing.Models;

namespace lesson5_MVC_Routing.Controllers;

public class HomeController
    (ILogger<HomeController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var incominConnection = HttpContext.Connection;
        var incomingAddress = incominConnection.RemoteIpAddress;

        logger.LogInformation($"[{incomingAddress}] has accessed the Index of Home!");

        return View(incomingAddress);
    }

    [HttpGet("/P/{id?}")]
    public IActionResult Privacy(int id = 0)
    {
        return View(id);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
