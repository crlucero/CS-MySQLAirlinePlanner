using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class City
    {
        private string _cityName;
        private int _id;

        public City(string cityName, int id = 0)
        {
           _cityName = cityName;
           _id = id;
        }

        public string GetCityName()
        {
            return _cityName;
        }

        public int GetId()
        {
            return _id;
        }

    

         public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (city) VALUES (@city);";
            MySqlParameter city = new MySqlParameter();
            city.ParameterName = "@city";
            city.Value = this._cityName;
            cmd.Parameters.Add(city);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"TRUNCATE TABLE cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int CityId = rdr.GetInt32(0);
                string CityName = rdr.GetString(1);
                City newCity = new City(CityName, CityId);
                allCities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City)otherCity;
                bool idEquality = this.GetId().Equals(newCity.GetId());
                bool nameEquality = this.GetCityName() == (newCity.GetCityName());
                return (idEquality && nameEquality);
            }
        }

        public static City Find(int id)
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int CityId = 0;
        string CityName = "";
        while(rdr.Read())
        {
            CityId = rdr.GetInt32(0);
            CityName = rdr.GetString(1);
        }
        City newCity = new City(CityName, CityId);
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newCity;
        }

        public List<Flight> GetFlights()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT flight_details.* FROM cities
            JOIN cities_flights ON (cities.id = cities_flights.city_id)
            JOIN flight_details ON (cities_flights.flight_id = flight_details.id)
            WHERE cities.id = @CityId;";
        MySqlParameter cityIdParameter = new MySqlParameter();
        cityIdParameter.ParameterName = "@CityId";
        cityIdParameter.Value = _id;
        cmd.Parameters.Add(cityIdParameter);
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Flight> flights = new List<Flight>{};
        while(rdr.Read())
        {
            int Id = rdr.GetInt32(0);
            string flightNumber = rdr.GetString(1);
            int departureCityId = rdr.GetInt32(2);
            int destinationCityId = rdr.GetInt32(3);
            string departureTime = rdr.GetString(4);
            string arrivalTime = rdr.GetString(5);
          Flight newFlight = new Flight(Id, flightNumber, departureCityId, destinationCityId, departureTime, arrivalTime);
          flights.Add(newFlight);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return flights;
        }

        public void AddFlight(Flight newFlight)
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";
        MySqlParameter city_id = new MySqlParameter();
        city_id.ParameterName = "@CityId";
        city_id.Value = _id;
        cmd.Parameters.Add(city_id);
        MySqlParameter flight_id = new MySqlParameter();
        flight_id.ParameterName = "@FlightId";
        flight_id.Value = newFlight.GetId();
        cmd.Parameters.Add(flight_id);
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        }

    }
}
