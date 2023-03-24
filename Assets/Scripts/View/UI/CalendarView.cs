using SunCalcNet;
using SunCalcNet.Model;
using SunCalcNet.Tests;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using CoordinateSharp;

[CLSCompliant(false)]
public class CalendarView : MonoBehaviour
{
    public Text sunriseText;
    public Text solarNoonText;
    public Text latText;
    public Text lngText;
    public Text westEraText;
    public Text buddhistEraText;
    public CalendarController controllerView;
    public Toggle mmCalToggle;
    public Toggle realCalToggle;

    // Start is called before the first frame update
    void Awake()
    {
        latText.text = "定位中...";
        lngText.text = "定位中...";
        sunriseText.text = "定位中...";
        solarNoonText.text = "定位中...";
        int isMMCal = SettingManager.Instance().GetCalType();
        if (isMMCal == 1)
        {
            mmCalToggle.isOn = true;
            realCalToggle.isOn = false;
        }
        else
        {
            mmCalToggle.isOn = false;
            realCalToggle.isOn = true;
        }
        mmCalToggle.onValueChanged.AddListener(OnMMToggleValueChanged);
    }
    void Start()
    {
        //SunCalcTests sunCalcTests = new SunCalcTests();
        //sunCalcTests.Get_Sun_Phases_Returns_Sun_Phases_For_The_Given_Date_And_Location();

        //GetSunTime(DateTime.Today);
        //float lat = 24;
        //float lng = 103;
        //(lat, lng) = CalendarManager.Instance().GetLocation();
        //latText.text = lat.ToString();
        //lngText.text = lng.ToString();

        // CalendarManager.Instance().StopLocation();
        SetEra(DateTime.Today);
    }
    void OnMMToggleValueChanged(bool value)
    {
        //Debug.LogError(value);
        if (value)
            SettingManager.Instance().SetCalType(1);
        else
            SettingManager.Instance().SetCalType(0);
        controllerView.Start();
    }
    public void SetEra(DateTime time)
    {
        westEraText.text = time.Year + "年" + time.Month + "月" + time.Day + "日";
        MyanmarDate myanmarDate = MyanmarDateConverter.convert(time.Year, time.Month, time.Day);
        buddhistEraText.text = myanmarDate.getBuddhistEraInt() + "年";
        //Debug.LogError(myanmarDate.getBuddhistEraInt());
    }
    //todo 整理到CalendarManager中
    public void GetSunTime(DateTime time)
    {
        SunPhase solarNoon = new SunPhase(SunPhaseName.SolarNoon, time);
        SunPhase sunrise = new SunPhase(SunPhaseName.Sunrise, time);//日出
        SunPhase dawn = new SunPhase(SunPhaseName.Dawn, time);//曙光升起
        SunPhase nauticalDawn = new SunPhase(SunPhaseName.NauticalDawn, time);//航海曙光
        //航海曙光+日出-曙光升起
        //初一十五

        //var date = new DateTime(2013, 3, 5, 0, 0, 0, DateTimeKind.Utc);
        //lat:是Latitude的简写,表示纬度。lng:是Longtitude的简写,表示经度
        //wikipali办公室当前经纬度
        float lat = 24;
        float lng = 103;
        (lat, lng) = CalendarManager.Instance().GetLocation();
        var height = 0;// 2000;
        TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(time);
        //Act
        var sunPhases = SunCalc.GetSunPhases(time, lat, lng, height, ts.Hours).ToList();


        var sunPhaseValueSolarNoon = sunPhases.First(x => x.Name.Value == solarNoon.Name.Value);
        var sunPhaseValueSunrise = sunPhases.First(x => x.Name.Value == sunrise.Name.Value);
        var sunPhaseValueDawn = sunPhases.First(x => x.Name.Value == dawn.Name.Value);
        var sunPhaseValueNauticalDawn = sunPhases.First(x => x.Name.Value == nauticalDawn.Name.Value);
        //航海曙光+日出-曙光升起
        TimeSpan tsd = new TimeSpan(sunPhaseValueSunrise.PhaseTime.Ticks - sunPhaseValueDawn.PhaseTime.Ticks);
        DateTime lightTime = sunPhaseValueNauticalDawn.PhaseTime + tsd;// sunPhaseValueSunrise.PhaseTime - sunPhaseValueDawn.PhaseTime;
        //var sunPhaseTime = sunPhaseValue.PhaseTime.ToString("yyyy-MM-dd hh:mm:ss");
        string sunPhaseTimeSolarNoon = sunPhaseValueSolarNoon.PhaseTime.ToString("HH:mm:ss");
        string sunPhaseTimeSunrise = lightTime.ToString("HH:mm:ss");
        sunriseText.text = sunPhaseTimeSunrise;
        solarNoonText.text = sunPhaseTimeSolarNoon;
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

    //    TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(time);
    //    Celestial cel = Celestial.CalculateCelestialTimes(lat, lng, time, ts.Hours);

    //    string sunPhaseTimeSunrise = cel.SunRise?.ToString("HH:mm:ss");
    //    string sunPhaseTimeSolarNoon = cel.SolarNoon?.ToString("HH:mm:ss");
    //    sunriseText.text = sunPhaseTimeSunrise;
    //    solarNoonText.text = sunPhaseTimeSolarNoon;
    //}
    // Update is called once per frame
    bool locationed = false;

    void Update()
    {

        if (!locationed && CalendarManager.Instance().isLocationed())
        {
            float lat = 24;
            float lng = 103;
            (lat, lng) = CalendarManager.Instance().GetLocation();
            latText.text = lat.ToString();
            lngText.text = lng.ToString();
            locationed = true;
            GetSunTime(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,0,1,0));
            //todo
            CalendarManager.Instance().StopLocation();
            controllerView.Start();
        }

    }
}
