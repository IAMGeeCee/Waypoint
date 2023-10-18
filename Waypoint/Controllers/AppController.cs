using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Waypoint.Controllers
{

   public class AppController : Controller
   {

      public IActionResult Map()
      {
         return View();
      }

      public IActionResult ShowDirections(string json)
      {
         return Content(json);
      }

      public async Task<IActionResult> CalculateDirections(string coordinates)
      {
         string apiKey = "88ed98eb-c3dc-4b84-be54-f3862db93f24";
         string apiUrl = $"https://graphhopper.com/api/1/route?point=51.5074,-0.1278&point={coordinates}&vehicle=car&locale=en&key={apiKey}";

         using (HttpClient client = new HttpClient())
         {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
               string json = await response.Content.ReadAsStringAsync();
               return Content(json);
            }
            else
            {
               throw new Exception($"GraphHopper API request failed with status code {response.StatusCode}");
            }
         }
      }


   }
}
