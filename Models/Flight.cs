using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        private int _id;
        private string _flightNumber;
        private int _departureCityId;
        private int _destinationCityId;
        private string _departureTime;
        private string _arrivalTime;
        // private string _flightStatus;

        public Flight(int id, string flightNumber,int departureCityId, int destinationCityId, string departureTime, string arrivalTime)
        {
            _id = id;
            _flightNumber = flightNumber;
            _departureCityId = departureCityId;
            _destinationCityId = destinationCityId;
            _departureTime = departureTime;
            _arrivalTime = arrivalTime;
        }

        public int GetId()
        {
            return _id;
        }
        public string GetFlightNumber()
        {
            return _flightNumber;
        }

        public int GetDepartureCityId()
        {
            return _departureCityId;
        }

        public int GetDestinationCityId()
        {
            return _destinationCityId;
        }

        public string GetDepartureTime()
        {
            return _departureTime;
        }

        public string GetArrivalTime()
        {
            return _arrivalTime;
        }

        public void Save()
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO flight_details ( flight_number,departure_city_id, destination_city_id, departure_time, arrival_time) VALUES (@flight_number, @departure_city_id, @destination_city_id, @departure_time, @arrival_time);";

        MySqlParameter flightNumber = new MySqlParameter();
        flightNumber.ParameterName = "@flight_number";
        flightNumber.Value = this._flightNumber;
        cmd.Parameters.Add(flightNumber);

        MySqlParameter departureCityId = new MySqlParameter();
        departureCityId.ParameterName = "@departure_city_id";
        departureCityId.Value = this._departureCityId;
        cmd.Parameters.Add(departureCityId);

        MySqlParameter destination_city_id = new MySqlParameter();
        destination_city_id.ParameterName = "@destination_city_id";
        destination_city_id.Value = this._destinationCityId;
        cmd.Parameters.Add(destination_city_id);

        MySqlParameter departure_time = new MySqlParameter();
        departure_time.ParameterName = "@departure_time";
        departure_time.Value = this._departureTime;
        cmd.Parameters.Add(departure_time);

        MySqlParameter arrival_time = new MySqlParameter();
        arrival_time.ParameterName = "@arrival_time";
        arrival_time.Value = this._arrivalTime;
        cmd.Parameters.Add(arrival_time);


        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        }

        public static List<Flight> GetAll()
        {
        List<Flight> allFlights = new List<Flight> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM flight_details;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
            int Id = rdr.GetInt32(0);
            string flightNumber = rdr.GetString(1);
            int departureCityId = rdr.GetInt32(2);
            int destinationCityId = rdr.GetInt32(3);
            string departureTime = rdr.GetString(4);
            string arrivalTime = rdr.GetString(5);

            Flight newFlight = new Flight(Id,flightNumber, departureCityId, destinationCityId,departureTime, arrivalTime);
            allFlights.Add(newFlight);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allFlights;
        }
   

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"TRUNCATE TABLE flight_details;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@cityId, @flightId);";
            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@cityId";
            city_id.Value = newCity.GetId();
            cmd.Parameters.Add(city_id);
            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@flightId";
            flight_id.Value = _id;
            cmd.Parameters.Add(flight_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<City> GetCities()
        {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT city_id FROM cities_flights WHERE flight_id = @flightId;";
        MySqlParameter flightIdParameter = new MySqlParameter();
        flightIdParameter.ParameterName = "@flightId";
        flightIdParameter.Value = _id;
        cmd.Parameters.Add(flightIdParameter);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<int> cityIds = new List<int> {};
        while(rdr.Read())
        {
            int cityId = rdr.GetInt32(0);
            cityIds.Add(cityId);
        }
        rdr.Dispose();
        List<City> cities = new List<City> {};
        foreach (int cityId in cityIds)
        {
            var cityQuery = conn.CreateCommand() as MySqlCommand;
            cityQuery.CommandText = @"SELECT * FROM cities WHERE id = @CityId;";
            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = cityId;
            cityQuery.Parameters.Add(cityIdParameter);
            var cityQueryRdr = cityQuery.ExecuteReader() as MySqlDataReader;
            while(cityQueryRdr.Read())
            {
            int thisCityId = cityQueryRdr.GetInt32(0);
            string cityName = cityQueryRdr.GetString(1);
            City foundCity = new City(cityName, thisCityId);
            cities.Add(foundCity);
            }
            cityQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return cities;
        }





    }
    
}