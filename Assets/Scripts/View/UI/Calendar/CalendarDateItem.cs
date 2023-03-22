using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using static CalendarController;
using System.Collections.Generic;

[CLSCompliant(false)]
public class CalendarDateItem : MonoBehaviour
{
    public Text dateText;
    public Text chineseDateText;
    public Text solarNoonText;
    public Image moon0Img;
    public Image moon1Img;
    public Image moon2Img;
    public Image moon3Img;
    public void Init(DateTime time)
    {
        Calendar calender = new Calendar(time);
        chineseDateText.text = calender.ChineseDayString;
        string chineseTwentyFourDay = calender.ChineseTwentyFourDay;
        if (!string.IsNullOrEmpty(chineseTwentyFourDay))
        {
            chineseDateText.text = "<b>" + chineseTwentyFourDay + "</b>";// calender.ChineseDayString;
        }
        MyanmarDate myanmarDate = MyanmarDateConverter.convert(time.Year, time.Month, time.Day);
        List<string> mhd = new List<string>();
        if (time.Month == 7)
        {
            TimeSpan d1 = new TimeSpan(1, 0, 0, 0);
            DateTime lastDay = time - d1;
            MyanmarDate myanmarDateLastDay = MyanmarDateConverter.convert(lastDay.Year, lastDay.Month, lastDay.Day);
            mhd = HolidayCalculator.myanmarHoliday(myanmarDate.myear, myanmarDate.mmonth, time.Month, myanmarDate.monthDay, myanmarDate.moonPhase, myanmarDateLastDay.moonPhase);
        }
        else
        {
            mhd = HolidayCalculator.myanmarHoliday(myanmarDate.myear, myanmarDate.mmonth, time.Month, myanmarDate.monthDay, myanmarDate.moonPhase, -1);
        }
        if (mhd != null && mhd.Count > 0)
        {
            //<color=#00ffffff>text</color>
            chineseDateText.text = "<color=#F13423><b>" + mhd[0] + "</b></color>";
        }
        //chineseDateText.text = "<color=#F13423><b>" + myanmarDate.mmonth + "</b></color>";
        //经纬度
        solarNoonText.text = CalendarManager.Instance().GetSunSolarNoonTime(time);
    }

    public void SetSolarNoonTextActive(bool active)
    {
        solarNoonText.gameObject.SetActive(active);
    }
    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
    }

    public void SetMoon(MoonType moonType)
    {
        moon0Img.gameObject.SetActive(false);
        moon1Img.gameObject.SetActive(false);
        moon2Img.gameObject.SetActive(false);
        moon3Img.gameObject.SetActive(false);
        switch (moonType)
        {
            case MoonType.Moon0:
                moon0Img.gameObject.SetActive(true);
                break;
            case MoonType.Moon1:
                moon1Img.gameObject.SetActive(true);
                break;
            case MoonType.Moon2:
                moon2Img.gameObject.SetActive(true);
                break;
            case MoonType.Moon3:
                moon3Img.gameObject.SetActive(true);
                break;
        }
    }
}
