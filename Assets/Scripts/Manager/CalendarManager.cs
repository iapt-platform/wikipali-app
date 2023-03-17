using SunCalcNet;
using SunCalcNet.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CalendarManager
{
    private CalendarManager() { }
    private static CalendarManager manager = null;
    //静态工厂方法 
    public static CalendarManager Instance()
    {
        if (manager == null)
        {
            manager = new CalendarManager();
        }
        return manager;
    }
    LocationService location = Input.location;
    public void StartLocation()
    {

        location.Start();
    }
    public void StopLocation()
    {
        location.Stop();
    }
    public (float, float) GetLocation()
    {
        location = Input.location;
#if UNITY_EDITOR
        return (24, 103);
#endif
        return (location.lastData.latitude, location.lastData.longitude);
    }
    //日中时间
    public string GetSunSolarNoonTime(DateTime time)//, float lat, float lng, float height = 0)
    {
        SunPhase solarNoon = new SunPhase(SunPhaseName.SolarNoon, time);

        TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(time);
        float lat = 24;
        float lng = 103;
        (lat, lng) = GetLocation();

        var height = 0;// 2000;
        //Act
        var sunPhases = SunCalc.GetSunPhases(time, lat, lng, height, ts.Hours).ToList();

        var sunPhaseValueSolarNoon = sunPhases.First(x => x.Name.Value == solarNoon.Name.Value);
        string sunPhaseTimeSolarNoon = sunPhaseValueSolarNoon.PhaseTime.ToString("HH:mm:ss");
        return sunPhaseTimeSolarNoon;
    }


    bool locationed = false;

    public bool isLocationed()
    {
#if UNITY_EDITOR
        return true;
#endif

        float lat = 24;
        float lng = 103;
        (lat, lng) = CalendarManager.Instance().GetLocation();
        if (lat == 0 && lng == 0)
            return false;
        else
            return true;

    }



}
