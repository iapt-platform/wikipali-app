using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using static CalendarController;

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
            chineseDateText.text = "<b>"+chineseTwentyFourDay+"</b>";// calender.ChineseDayString;
        }

        //经纬度
        solarNoonText.text = CalendarManager.Instance().GetSunSolarNoonTime(time, 24, 103);
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
