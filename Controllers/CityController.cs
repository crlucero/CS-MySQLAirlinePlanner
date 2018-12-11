using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class CityController : Controller
    {
        [HttpGet("/cities")]
        public ActionResult Index()
        {
            List<City> allCities = City.GetAll();
            return View(allCities);
        }

        [HttpGet("/cities/new")]
        public ActionResult New()
        {
           return View();
        }

        [HttpPost("/cities")]
        public ActionResult Create(string cityName)
        {
            City newCity = new City(cityName);
            newCity.Save();
            
            List<City> allCities = City.GetAll();
            return View("Index", allCities);
        }

        [HttpPost("/cities/{cityId}/flights")]
        public ActionResult Create(int id, string flightNumber,int departureCityId, int destinationCityId, string departureTime, string arrivalTime)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            City foundCity = City.Find(id);
            Flight newFlight = new Flight(id, flightNumber, departureCityId, destinationCityId,departureTime, arrivalTime);
            newFlight.Save(); 
            foundCity.AddFlight(newFlight);
            List<Flight> cityFlights = foundCity.GetFlights();
            model.Add("flights", cityFlights);
            model.Add("city", foundCity);
            return View("Show", model);
        }



         [HttpGet("/cities/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            City selectedCity = City.Find(id);
            List<Flight> CityFlights = selectedCity.GetFlights();
            model.Add("city", selectedCity);
            model.Add("flights", CityFlights);
            return View(model);
        }
    }
    
}