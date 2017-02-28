using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AirlinePlanner
{
  public class Flight
  {
    private int _id;
    private string _flightNumber;
    private DateTime _dateTime;
    private string _status;

    public Flight(string FlightNumber, DateTime departTime, string Status, int Id = 0)
    {
      _id = Id;
      _flightNumber = FlightNumber;
      _dateTime = departTime;
      _status = Status;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFlightNumber()
    {
      return _flightNumber;
    }

    public DateTime GetDate()
    {
      return _dateTime;
    }

    public string GetStatus()
    {
      return _status;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if(!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = (this.GetId() == newFlight.GetId());
        bool flightnumberEquality = (this.GetFlightNumber() == newFlight.GetFlightNumber());
        bool flightDateEquality = (this.GetDate() == newFlight.GetDate());
        bool statusEquality = (this.GetStatus() == newFlight.GetStatus());
        return (idEquality && flightnumberEquality && flightDateEquality && statusEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM flight;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flight;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightNumber = rdr.GetString(1);
        DateTime flightTime = rdr.GetDateTime(2);
        string flightStatus = rdr.GetString(3);
        Flight newFlight = new Flight(flightNumber, flightTime, flightStatus, flightId);
        allFlights.Add(newFlight);
      }

        if (rdr != null)
        {
          rdr.Close();
        }
        if (conn != null)
        {
          conn.Close();
        }

      return allFlights;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flight (flight_number, date_time, status) OUTPUT INSERTED.id VALUES (@FlightNumber, @FlightDateTime, @FlightStatus);", conn);

      SqlParameter FlightNumberParameter = new SqlParameter("@FlightNumber", this.GetFlightNumber());
      SqlParameter FlightDateTimeParameter = new SqlParameter("@FlightDateTime", this.GetDate());
      SqlParameter FlightStatusParameter = new SqlParameter("@FlightStatus", this.GetStatus());

      cmd.Parameters.Add(FlightNumberParameter);
      cmd.Parameters.Add(FlightDateTimeParameter);
      cmd.Parameters.Add(FlightStatusParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
