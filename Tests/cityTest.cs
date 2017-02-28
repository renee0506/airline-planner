using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class AirlineTest : IDisposable
  {
    public AirlineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = City.GetAll().Count;

      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      City firstCity = new City("Seattle");
      City secondCity = new City("Seattle");

      Assert.Equal(firstCity, secondCity);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      City testCity = new City("Seattle");

      testCity.Save();
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignIdToObject()
    {
      City testCity = new City("Seattle");

      testCity.Save();
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      Assert.Equal(result, testId);
    }

    [Fact]
    public void Test_Find_FindsCityInDatabase()
    {
      City testCity = new City("Seattle");
      testCity.Save();

      City foundCity = City.Find(testCity.GetId());

      Assert.Equal(testCity, foundCity);
    }

    // [Fact]
    // public void Test_AddFlight_AddsFlightToCity()
    // {
    //   City testCity = new City("Seattle");
    //   testCity.Save();
    //
    //   Flight testFlight = new Flight("")
    // }
  }
}
