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

    //日中时间
    public string GetSunSolarNoonTime(DateTime time, float lat, float lng,float height = 0)
    {
        SunPhase solarNoon = new SunPhase(SunPhaseName.SolarNoon, time);

        TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(time);
        //Act
        var sunPhases = SunCalc.GetSunPhases(time, lat, lng, height, ts.Hours).ToList();

        var sunPhaseValueSolarNoon = sunPhases.First(x => x.Name.Value == solarNoon.Name.Value);
        string sunPhaseTimeSolarNoon = sunPhaseValueSolarNoon.PhaseTime.ToString("HH:mm:ss");
        return sunPhaseTimeSolarNoon;
    }







}
