using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
