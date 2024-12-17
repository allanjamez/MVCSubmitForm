using System;
using System.Diagnostics;
using System.Text.Json;
using BidOneWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BidOneWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _filePath = "Data/formdata.json";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SubmitModel SubmitDetails)
        {
            if (ModelState.IsValid)
            {            
                if (!Directory.Exists("Data"))
                {
                    Directory.CreateDirectory("Data");
                }

                var jsonData = JsonSerializer.Serialize(SubmitDetails);
                System.IO.File.AppendAllText(_filePath, jsonData + Environment.NewLine);

                ViewBag.Message = "Form submitted successfully!";

                ModelState.Clear();
            }
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}