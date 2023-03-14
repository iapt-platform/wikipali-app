using CoordinateSharp;
using SunCalcNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using Xunit;

namespace SunCalcNet.Tests
{
    public class SunCalcTests
    {
        //[Fact]
        public void Get_Sun_Position_Returns_Azimuth_And_Altitude_For_The_Given_Time_And_Location()
        {
            //Arrange
            var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
            var lat = 50.5;
            var lng = 30.5;

            //Act
            var sunPosition = SunCalc.GetSunPosition(date, lat, lng);

            //Assert
            ////Assert.Equal(-2.5003175907168385, sunPosition.Azimuth, 15);
            ////Assert.Equal(-0.7000406838781611, sunPosition.Altitude, 15);
        }

        //[Fact]
        public void Get_Sun_Phases_Returns_Sun_Phases_For_The_Given_Date_And_Location()
        {
            var heightTestData = new List<SunPhase>
            {
                new SunPhase(SunPhaseName.SolarNoon, DateTime.Now),
                new SunPhase(SunPhaseName.Nadir,  DateTime.Now),
                new SunPhase(SunPhaseName.Sunrise, DateTime.Now),
                new SunPhase(SunPhaseName.Sunset,  DateTime.Now),

                new SunPhase(SunPhaseName.SolarNoon, DateTime.UtcNow),
                new SunPhase(SunPhaseName.Nadir,  DateTime.UtcNow),
                new SunPhase(SunPhaseName.Sunrise, DateTime.UtcNow),
                new SunPhase(SunPhaseName.Sunset,  DateTime.UtcNow),
            };

            var date = DateTime.UtcNow;// new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
            var lat = 24;
            var lng = 103;
            var height = 2000;
            TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(date);
            Celestial cel = Celestial.CalculateCelestialTimes(lat, lng, date, ts.Hours);
            Debug.LogError("   SunRise   " + cel.SunRise);
            Debug.LogError("   SolarNoon   " + cel.SolarNoon);

            //Act
            var sunPhases = SunCalc.GetSunPhases(date, lat, lng, height).ToList();

            //Assert
            foreach (var testSunPhase in heightTestData)
            {
                var sunPhaseValue = sunPhases.First(x => x.Name.Value == testSunPhase.Name.Value);

                var testDataPhaseTime = testSunPhase.PhaseTime.ToString("yyyy-MM-dd HH:mm:ss");
                var sunPhaseTime = sunPhaseValue.PhaseTime.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.LogError(testSunPhase.Name + "   ----   " + testDataPhaseTime + "  ----  " + sunPhaseTime + cel.SunRise);
                ////Assert.Equal(testDataPhaseTime, sunPhaseTime);
            }

        }

        //[Fact]
        public void Get_Sun_Phases_Works_At_North_Pole()
        {
            //Arrange
            var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
            var lat = 90;
            var lng = 135;

            //Act
            var sunPhases = SunCalc.GetSunPhases(date, lat, lng).ToList();

            //Assert
            ////Assert.Equal(2, sunPhases.Count);
        }

        //[Fact]
        public void Get_Sun_Phases_Adjusts_Sun_Phases_When_Additionally_Given_The_Observer_Height()
        {
            //Arrange
            var heightTestData = new List<SunPhase>
            {
                new SunPhase(SunPhaseName.SolarNoon, new DateTime(2013, 3, 5, 10, 10, 57, DateTimeKind.Utc)),
                new SunPhase(SunPhaseName.Nadir, new DateTime(2013, 3, 4, 22, 10, 57, DateTimeKind.Utc)),
                new SunPhase(SunPhaseName.Sunrise, new DateTime(2013, 3, 5, 4, 25, 7, DateTimeKind.Utc)),
                new SunPhase(SunPhaseName.Sunset, new DateTime(2013, 3, 5, 15, 56, 46, DateTimeKind.Utc))
            };

            var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
            var lat = 50.5;
            var lng = 30.5;
            var height = 2000;

            //Act
            var sunPhases = SunCalc.GetSunPhases(date, lat, lng, height).ToList();

            //Assert
            foreach (var testSunPhase in heightTestData)
            {
                var sunPhaseValue = sunPhases.First(x => x.Name.Value == testSunPhase.Name.Value);

                var testDataPhaseTime = testSunPhase.PhaseTime.ToString("yyyy-MM-dd hh:mm:ss");
                var sunPhaseTime = sunPhaseValue.PhaseTime.ToString("yyyy-MM-dd hh:mm:ss");
                ////Assert.Equal(testDataPhaseTime, sunPhaseTime);
            }
        }
    }
}