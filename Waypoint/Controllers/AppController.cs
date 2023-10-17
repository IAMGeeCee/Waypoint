using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Waypoint.Controllers;

public class AppController : Controller
{
   public IActionResult Map(){
    return View();
   }
}
