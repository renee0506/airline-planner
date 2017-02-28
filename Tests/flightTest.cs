using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class FlightTest : IDisposable
  {
    public FlightTest()
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
    int result = Flight.GetAll().Count;

    Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Flight firstFlight = new Flight("B20", date1, "Delayed");
      Flight secondFlight = new Flight("B20", date1, "Delayed");

      Assert.Equal(firstFlight, secondFlight);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Flight testFlight = new Flight("B20", date1, "Delayed");

      testFlight.Save();
      List<Flight> result = Flight.GetAll();
      List<Flight> expected = new List<Flight>{testFlight};

      Assert.Equal(expected, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      DateTime date1 = new DateTime(2008, 4, 10);
      Flight testFlight = new Flight("B20", date1, "Delayed");

      testFlight.Save();
      Flight savedFlight = Flight.GetAll()[0];
      int result = savedFlght.GetId();
      int testId = testFlight.GetId();

      Assert.Equal(testId, result);
    }

  }
}
