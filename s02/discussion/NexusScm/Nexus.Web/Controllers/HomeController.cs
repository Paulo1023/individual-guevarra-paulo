using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nexus.Web.Models;

namespace Nexus.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //returns index view
    public IActionResult Index()
    {
        return View();
    }

    //iaction is the method to run the views
    public IActionResult Privacy()
    {
        //returns a variable key value paired
        ViewData["PrivacyMessage"] = "This policy was last updated on today's date.";
        //returns the view inside the folder "Home" with view file same name as the IActionResult name
        return View();
    }

    //if file not exists in "Home" folder it will look in the "Shared" folder
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
