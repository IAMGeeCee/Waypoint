using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Waypoint.Controllers;

public class HomeController : Controller
{
   public IActionResult Index(){
    return View();
   }
}
