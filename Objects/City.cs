using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace AirlinePlanner
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = (this.GetId() == newCity.GetId());
        bool nameEquality = (this.GetName() == newCity.GetName());
        return (idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }

      if (rdr !=null )
      {
        rdr.Close();
      }
      if (conn != null )
      {
        conn.Close();
      }

      return allCities;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = this.GetName();

      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
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

    public static City Find(int Id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @cityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter("@cityId", Id);
      cmd.Parameters.Add(cityIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string foundName = null;

      while(rdr.Read())
      {
          foundId = rdr.GetInt32(0);
          foundName = rdr.GetString(1);
      }

      City foundCity = new City(foundName, foundId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCity;
    }
  }
}
