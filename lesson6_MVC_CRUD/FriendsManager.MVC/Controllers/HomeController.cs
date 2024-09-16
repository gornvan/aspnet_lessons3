using System.Diagnostics;
using FriendsManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FriendsManager.MVC.Controllers;

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

    [HttpGet]
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
