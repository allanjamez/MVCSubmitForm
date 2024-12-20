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
                // Ensure the "Data" folder exists
                if (!Directory.Exists("Data"))
                {
                    Directory.CreateDirectory("Data");
                }

                List<SubmitModel> data = new List<SubmitModel>();

       
                if (System.IO.File.Exists(_filePath))
                {
                    string fileContent = System.IO.File.ReadAllText(_filePath);

                    try
                    {
                  
                        data = JsonSerializer.Deserialize<List<SubmitModel>>(fileContent) ?? new List<SubmitModel>();
                    }
                    catch (JsonException)
                    {
               
                        string[] lines = System.IO.File.ReadAllLines(_filePath);
                        foreach (string line in lines)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                try
                                {
                                    SubmitModel? model = JsonSerializer.Deserialize<SubmitModel>(line.Trim());
                                    if (model != null)
                                    {
                                        data.Add(model);
                                    }
                                }
                                catch (JsonException)
                                {
                                }
                            }
                        }
                    }
                }

                data.Add(SubmitDetails);

                string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_filePath, jsonData);

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
