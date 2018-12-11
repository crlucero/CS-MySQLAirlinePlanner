using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class FlightController : Controller
    {
        [HttpGet("/flights")]
        public ActionResult Index()
        {
            List<Flight> allFlights = Flight.GetAll();
            return View(allFlights);
        }

        [HttpGet("cities/flights/new")]
        public ActionResult New()
        {
           return View();
        }

        [HttpPost("/flights")]
        public ActionResult Create(int id,string flightNumber,int departureCityId, int destinationCityId, string departureTime, string arrivalTime)
        {
            Flight newFlight = new Flight(id,flightNumber,departureCityId,destinationCityId,departureTime,arrivalTime);
            newFlight.Save();
            List<Flight> allFlights = Flight.GetAll();
            return View("Index", allFlights);

        }
    }
    
}