﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

      public IActionResult MobileMenu(){
         return View();
      }

      public async Task<IActionResult> CalculateDirections(string coordinates)
      {
         double Longitude;
         Longitude = Convert.ToDouble(HttpContext.Session.GetString("CurrentLocation").Split(',')[0]);
         double Latitude = Convert.ToDouble(HttpContext.Session.GetString("CurrentLocation").Split(',')[1]);


         string apiKey = "88ed98eb-c3dc-4b84-be54-f3862db93f24";
         string apiUrl = $"https://graphhopper.com/api/1/route?point={Longitude},{Latitude}&point={coordinates}&vehicle=car&locale=en&key={apiKey}";
         string otherUrl = $"https://graphhopper.com/api/1/route?profile=car&point={Longitude},{Latitude}&point={coordinates}&curbside=any&locale=en&elevation=false&optimize=false&instructions=true&calc_points=true&debug=false&points_encoded=true&ch.disable=false&heading=0&heading_penalty=120&pass_through=false&algorithm=round_trip&round_trip.distance=10000&round_trip.seed=0&alternative_route.max_paths=2&alternative_route.max_weight_factor=1.4&alternative_route.max_share_factor=0.6&key={apiKey}";

         using (HttpClient client = new HttpClient())
         {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
               string json = await response.Content.ReadAsStringAsync();
               return View("Map",json);
            }
            else
            {
               throw new Exception($"GraphHopper API request failed with status code {response.StatusCode}");
            }
         }
      }


   }
}