using SunCalcNet;
using SunCalcNet.Model;
using SunCalcNet.Tests;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoordinateSharp;

[CLSCompliant(false)]
public class CalendarView : MonoBehaviour
{
    public Text sunriseText;
    public Text solarNoonText;
    // Start is called before the first frame update
    void Start()
    {
        //    SunCalcTests sunCalcTests = new SunCalcTests();
        //    sunCalcTests.Get_Sun_Phases_Returns_Sun_Phases_For_The_Given_Date_And_Location();
        GetSunTime(DateTime.Today);
    }
    //public void GetSunTime(DateTime time)
    //{
    //    SunPhase solarNoon = new SunPhase(SunPhaseName.SolarNoon, time);
    //    SunPhase sunrise = new SunPhase(SunPhaseName.Sunrise, time);


    //    //var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
    //    //lat:是Latitude的简写,表示纬度。lng:是Longtitude的简写,表示经度
    //    //wikipali办公室当前经纬度
    //    var lat = 24;
    //    var lng = 103;
    //    var height = 2000;

    //    //Act
    //    var sunPhases = SunCalc.GetSunPhases(time, lat, lng, height).ToList();


    //    var sunPhaseValueSolarNoon = sunPhases.First(x => x.Name.Value == solarNoon.Name.Value);
    //    var sunPhaseValueSunrise = sunPhases.First(x => x.Name.Value == sunrise.Name.Value);

    //    //var sunPhaseTime = sunPhaseValue.PhaseTime.ToString("yyyy-MM-dd hh:mm:ss");
    //    string sunPhaseTimeSolarNoon = sunPhaseValueSolarNoon.PhaseTime.ToString("HH:mm:ss");
    //    string sunPhaseTimeSunrise = sunPhaseValueSunrise.PhaseTime.ToString("HH:mm:ss");
    //    sunriseText.text = sunPhaseTimeSunrise;
    //    solarNoonText.text = sunPhaseTimeSolarNoon;
    //}
    public void GetSunTime(DateTime time)
    {
        SunPhase solarNoon = new SunPhase(SunPhaseName.SolarNoon, time);
        SunPhase sunrise = new SunPhase(SunPhaseName.Sunrise, time);


        //var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
        //lat:是Latitude的简写,表示纬度。lng:是Longtitude的简写,表示经度
        //wikipali办公室当前经纬度
        var lat = 24;
        var lng = 103;
        var height = 2000;

        TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(time);
        Celestial cel = Celestial.CalculateCelestialTimes(lat, lng, time, ts.Hours);

        string sunPhaseTimeSunrise = cel.SunRise?.ToString("HH:mm:ss");
        string sunPhaseTimeSolarNoon = cel.SolarNoon?.ToString("HH:mm:ss");
        sunriseText.text = sunPhaseTimeSunrise;
        solarNoonText.text = sunPhaseTimeSolarNoon;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
