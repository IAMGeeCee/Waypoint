﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;

namespace Waypoint.Controllers
{
   public class LocationData
   {
      public double Latitude { get; set; }
      public double Longitude { get; set; }
   }

   public class PostcodeInfo
   {
      public int status { get; set; }
      public Result result { get; set; }
   }

   public class Result
   {
      public double latitude { get; set; }
      public double longitude { get; set; }
   }


   public class AppController : Controller
   {
      [HttpPost]
      public IActionResult ReceiveLocation([FromBody] string data)
      {
         HttpContext.Session.SetString("CurrentLocation", data);
         return Content("Data received successfully: " + data);
      }

      public IActionResult Index()
      {
         return View();
      }

      public IActionResult Map()
      {
         return View();
      }

      public IActionResult Car()
      {
         return View();
      }

      public IActionResult MobileMenu()
      {
         return View();
      }

      public async Task<IActionResult> CalculateDirections(string currentcoordinates, string coordinates)
      {
         double Longitude = 0;
         double Latitude = 0;
         
         if (coordinates.Contains(','))
         {
            Longitude = Convert.ToDouble(currentcoordinates.Split(',')[0]);
            Latitude = Convert.ToDouble(currentcoordinates.Split(',')[1]);
         }
         else
         {
            //POSTCODE

            string apiURL = $"https://api.postcodes.io/postcodes/{coordinates}";

            using (HttpClient client = new HttpClient())
            {
               HttpResponseMessage response = await client.GetAsync(apiURL);

               if (response.IsSuccessStatusCode)
               {
                  string json = await response.Content.ReadAsStringAsync();

                  // Deserialize the JSON response
                  var postcodeInfo = JsonConvert.DeserializeObject<PostcodeInfo>(json);

                  if (postcodeInfo != null && postcodeInfo.result != null)
                  {
                     Latitude = postcodeInfo.result.latitude;
                     Longitude = postcodeInfo.result.longitude;

                     // Now, you have latitude and longitude as doubles
                  }
               }
               else
               {
                  throw new Exception($"Postcode API request failed with status code {response.StatusCode}");
               }
            }
         }


         string apiKey = "88ed98eb-c3dc-4b84-be54-f3862db93f24";
         string apiUrl = $"https://graphhopper.com/api/1/route?point={currentcoordinates}&point={Latitude},{Longitude}&vehicle=car&locale=en&key={apiKey}&instructions=true&point_hint=0.0&point_hint=1.0";

         using (HttpClient client = new HttpClient())
         {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
               string json = await response.Content.ReadAsStringAsync();
               return View("Map", json);
            }
            else
            {
               throw new Exception($"GraphHopper API request failed with status code {response.StatusCode}");
            }
         }
      }


   }
}