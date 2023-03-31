//package mmcalendar;

//import java.util.ArrayList;
//import java.util.List;


using System;
using System.Collections.Generic;
/**
* Holiday Calculator 
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0
*
*/
public /*final*/ class HolidayCalculator
{

    /**
	 * Don't let anyone instantiate this class.
	 */
    private HolidayCalculator()
    {
    }

    /**
	 * Eid
	 */
    private static /*final*/ int[] ghEid2 = new int[] { 2456936, 2457290, 2457644, 2457998, 2458353 };

    /**
	 * Chinese New Year ref
	 * http://www.mom.gov.sg/employment-practices/public-holidays ref
	 * https://en.wikipedia.org/wiki/Chinese_New_Year
	 */
    private static /*final*/ int[] ghCNY = new int[] { 2456689, 2456690, 2457073, 2457074, 2457427, 2457428, 2457782,
            2457783, 2458166, 2458167 };

    private static /*final*/ int[] ghDiwali = new int[] { 2456599, 2456953, 2457337, 2457691, 2458045, 2458429 };
    private static /*final*/ int[] ghEid = new int[] { 2456513, 2456867, 2457221, 2457576, 2457930, 2458285 };

    /**
	 * Check for English Holiday
	 * 
	 * @param gy
	 *            year
	 * @param gm
	 *            month [Jan=1, ... , Dec=12]
	 * @param gd
	 *            day [0-31]
	 * @return Name of Holiday Strings List if exist
	 */
    static List<string> englishHoliday(int gy, int gm, int gd)
    {

        List<string> holiday = new List<string>();

        if ((gy >= 2018) && (gm == 1) && (gd == 1))
        {
            holiday.Add("New Year Day");
        }
        else if ((gy >= 1948) && (gm == 1) && (gd == 4))
        {
            holiday.Add("Independence Day");
        }
        else if ((gy >= 1947) && (gm == 2) && (gd == 12))
        {
            holiday.Add("Union Day");
        }
        else if ((gy >= 1958) && (gm == 3) && (gd == 2))
        {
            holiday.Add("Peasants Day");
        }
        else if ((gy >= 1945) && (gm == 3) && (gd == 27))
        {
            holiday.Add("Resistance Day");
        }
        else if ((gy >= 1923) && (gm == 5) && (gd == 1))
        {
            holiday.Add("Labour Day");
        }
        else if ((gy >= 1947) && (gm == 7) && (gd == 19))
        {
            holiday.Add("Martyrs Day");
        }
        else if ((gm == 12) && (gd == 25))
        {
            holiday.Add("Christmas Day");
        }
        else if ((gy == 2017) && (gm == 12) && (gd == 30))
        {
            holiday.Add("Holiday");
        }
        else if ((gy >= 2017) && (gm == 12) && (gd == 31))
        {
            holiday.Add("Holiday");
        }

        return holiday;
    }

    /**
	 * Check for Myanmar Holiday
	 * 
	 * @param myear
	 *            Myanmar Year
	 * @param mmonth
	 *            Myanmar month [Tagu=1, ... , Tabaung=12]
	 * @param monthDay
	 *            Myanmar Month day [0-30]
	 * @param moonPhase
	 *            Moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon]
	 * @return Name of Holiday Strings List if exist
	 */
    //获取缅甸节日
    public static List<string> myanmarHoliday(double myear, int mmonth, int westMonth, int monthDay, int moonPhase, int lastDayMoonPhase)
    {

        List<string> holiday = new List<string>();

        if ((mmonth == 2) && (moonPhase == 1))
        {
            //holiday.Add("Buddha Day");//敬佛节
            holiday.Add("敬佛节");//敬佛节
        }
        //西历2月
        else if ((westMonth == 2) && (moonPhase == 1))
        {
            holiday.Add("敬僧节");
        }
        // Vesak day
        //Full Moon Day of Waso
        //else if ((mmonth == 4) && (moonPhase == 1))
        //西历7月
        else if ((westMonth == 7) && (moonPhase == 1))
        {
            holiday.Add("敬法节");
        }
        else if ((westMonth == 7) && (lastDayMoonPhase == 1))
        {
            holiday.Add("入雨安居");//入雨安居
            //holiday.Add("Start of Buddhist Lent");//入雨安居
        }
        // Warso day
        else if ((mmonth == 7) && (moonPhase == 1))
        {
            holiday.Add("出雨安居");//出雨安居
            //holiday.Add("End of Buddhist Lent");//出雨安居
        }
        //else if ((myear >= 1379) && (mmonth == 7) && (monthDay == 14 || monthDay == 16))
        //{
        //    holiday.Add("Holiday");
        //}
        //else if ((mmonth == 8) && (moonPhase == 1))
        //{
        //    holiday.Add("Tazaungdaing");
        //}
        //else if ((myear >= 1379) && (mmonth == 8) && (monthDay == 14))
        //{
        //    holiday.Add("Holiday");
        //}
        //else if ((myear >= 1282) && (mmonth == 8) && (monthDay == 25))
        //{
        //    holiday.Add("National Day");
        //}
        //else if ((mmonth == 10) && (monthDay == 1))
        //{
        //    holiday.Add("Karen New Year Day");
        //}
        //else if ((mmonth == 12) && (moonPhase == 1))
        //{
        //    holiday.Add("Tabaung Pwe");
        //}

        return holiday;
    }

    //获取农历节日
    public static List<string> farmerHoliday(int fmonth, string fday, bool isLeapYear, bool isLeapMon, int leapMon)
    {
        //农历 三月15 敬佛节
        List<string> holiday = new List<string>();

        if ((fmonth == 3) && (fday == "十五"))
        {
            holiday.Add("敬佛节");//敬佛节 // Vesak day
        }
        //农历  8月15 出雨安居
        else if (!isLeapMon && (fmonth == 8) && (fday == "十五"))
        {
            holiday.Add("出雨安居");
        }
        //农历  8月15倒退三个月 入雨安居
        if (isLeapYear)
        {
            //闰月的情况
            if (leapMon == 8)
            {
                if ((fmonth == 5) && (fday == "十五"))
                {
                    holiday.Add("入雨安居");
                }
            }
            else if (leapMon == 7)
            {
                if ((fmonth == 6) && (fday == "十五"))
                {
                    holiday.Add("入雨安居");
                }
            }
            else if (leapMon == 6)
            {
                if (!isLeapMon && (fmonth == 6) && (fday == "十五"))
                {
                    holiday.Add("入雨安居");
                }
            }
            else if (leapMon == 5)
            {
                if (isLeapMon && (fmonth == 5) && (fday == "十五"))
                {
                    holiday.Add("入雨安居");
                }
            }
            else
            {
                if ((fmonth == 5) && (fday == "十五"))
                {
                    holiday.Add("入雨安居");
                }
            }
        }
        else
        {
            if ((fmonth == 5) && (fday == "十五"))
            {
                holiday.Add("入雨安居");
            }
        }

        //else if ((mmonth == 7) && (moonPhase == 1))
        //{
        //    holiday.Add("出雨安居");//出雨安居
        //    //holiday.Add("End of Buddhist Lent");//出雨安居
        //}


        return holiday;
    }
    //获取如实历节日
    //预先计算一年节日
    public static Dictionary<DateTime, string> preYearTrueHoliday(int year)
    {
        Dictionary<DateTime, string> res = new Dictionary<DateTime, string>();
        TimeSpan day1 = new TimeSpan(1, 0, 0, 0);
        //4月16日最近的月圆日，卫塞节(敬佛节)
        DateTime dateW = new DateTime(year, 4, 16);
        if (CalendarController.GetMoonType(dateW) == CalendarController.MoonType.Moon2)
        {
            res.Add(dateW, "敬佛节");
        }
        else
        {
            for (int i = 1; i < 15; i++)
            {
                TimeSpan dayPass = new TimeSpan(i, 0, 0, 0);
                DateTime dateWP = dateW - dayPass;
                DateTime dateWN = dateW + dayPass;
                if (dateWP.Day == 6)
                    ;
                if (CalendarController.GetMoonType(dateWP) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateWP, "敬佛节");
                    break;
                }
                if (CalendarController.GetMoonType(dateWN) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateWN, "敬佛节");
                    break;
                }
            }
        }
        //出雨安居 9月27日最近的月圆日
        DateTime dateC = new DateTime(year, 9, 27);
        if (CalendarController.GetMoonType(dateC) == CalendarController.MoonType.Moon2)
        {
            res.Add(dateC, "出雨安居");
        }
        else
        {
            for (int i = 1; i < 15; i++)
            {
                TimeSpan dayPass = new TimeSpan(i, 0, 0, 0);
                DateTime dateCP = dateC - dayPass;
                DateTime dateCN = dateC + dayPass;
                if (CalendarController.GetMoonType(dateCP) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateCP, "出雨安居");
                    dateC = dateCP;
                    break;
                }
                if (CalendarController.GetMoonType(dateCN) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateCN, "出雨安居");
                    dateC = dateCN;
                    break;
                }
            }
        }
        //出雨安居 的前三个月圆日的后一天是入雨安居
        DateTime dateR = new DateTime(dateC.Year, dateC.Month - 3, dateC.Day);
        if (CalendarController.GetMoonType(dateR) == CalendarController.MoonType.Moon2)
        {
            res.Add(dateR + day1, "入雨安居");
        }
        else
        {
            for (int i = 1; i < 15; i++)
            {
                TimeSpan dayPass = new TimeSpan(i, 0, 0, 0);
                DateTime dateRP = dateR - dayPass;
                DateTime dateRN = dateR + dayPass;
                if (CalendarController.GetMoonType(dateRP) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateRP + day1, "入雨安居");
                    break;
                }
                if (CalendarController.GetMoonType(dateRN) == CalendarController.MoonType.Moon2)
                {
                    res.Add(dateRN + day1, "入雨安居");
                    break;
                }
            }
        }
        return res;
    }
    //获取如实历节日
    public static List<string> trueHoliday(Dictionary<DateTime, string> yearHoliday, DateTime date)
    {
        List<string> holiday = new List<string>();
        DateTime newDate = new DateTime(date.Year, date.Month, date.Day);

        if (yearHoliday.ContainsKey(newDate))
        {
            holiday.Add(yearHoliday[newDate]);
        }

        return holiday;
    }
    /**
     * 
     * @param jdn
     *            Julian Day Number to check
     * @param myear
     *            Myanmar year
     * @param monthType
     *            Myanmar month type [oo=0, hnaung=1
     * @return List of holiday string
     */
    public static List<string> thingyan(double jdn, double myear, int monthType)
    {

        // start of Thingyan
        int BGNTG = 1100;

        List<string> holiday = new List<string>();

        // double atat;
        double akn, atn;
        // start of third era
        int SE3 = 1312;

        double ja = Constants2.SY * (myear + monthType) + Constants2.MO;
        double jk;

        if (myear >= SE3)
        {
            jk = ja - 2.169918982;
        }
        else
        {
            jk = ja - 2.1675;
        }

        akn = Math.Round(jk);
        atn = Math.Round(ja);

        // if (jdn == (atn + 1))
        if (Math.Abs(jdn - (atn + 1)) < 0.0000001)
        {
            holiday.Add("Myanmar New Year Day");
        }

        if ((myear + monthType) >= BGNTG)
        {
            if (jdn == atn)
            {
                holiday.Add("Thingyan Atat");
            }
            else if ((jdn > akn) && (jdn < atn))
            {
                holiday.Add("Thingyan Akyat");
            }
            else if (jdn == akn)
            {
                holiday.Add("Thingyan Akya");
            }
            else if (jdn == (akn - 1))
            {
                holiday.Add("Thingyan Akyo");
            }
            else if (((myear + monthType) >= 1369) && ((myear + monthType) < 1379)
                  && ((jdn == (akn - 2)) || ((jdn >= (atn + 2)) && (jdn <= (akn + 7)))))
            {
                holiday.Add("Holiday");
            }
        }

        return holiday;
    }

    /**
	 * Other holidays (ohol) Diwali or Eid
	 * 
	 * @param jd
	 *            Julian day number
	 * @return List of holiday string
	 */
    public static List<string> otherHolidays(double jd)
    {

        List<string> holiday = new List<string>();

        if (BinarySearchUtil.search(jd, ghDiwali) >= 0)
        {
            holiday.Add("Diwali");
        }
        if (BinarySearchUtil.search(jd, ghEid) >= 0)
        {
            holiday.Add("Eid");
        }

        return holiday;
    }

    /**
	 * Anniversary day
	 * 
	 * @param jd
	 *            Julian Day Number,
	 * @param calendarType
	 *            calendar type [Optional argument: 0=english (default),
	 *            1=Gregorian, 2=Julian]
	 * @return
	 * 
	 * 		dependency: DoE(), j2w()
	 */
    //public static List<string> ecd(double jd, CalendarType calendarType) {
    //	// ct=ct||0;
    //	if (calendarType == null) {
    //		calendarType = CalendarType.ENGLISH;
    //	}

    //	List<string> holiday = new List<string>();

    //	WesternDate wd = WesternDateConverter.convert(jd, calendarType);
    //	double doe = DoE(wd.getYear());

    //	if ((wd.getYear() <= 2017) && (wd.getMonth() == 1) && (wd.getDay() == 1)) {
    //		holiday.Add( "New Year Day");
    //	} else if ((wd.getYear() >= 1915) && (wd.getMonth() == 2) && (wd.getDay() == 13)) {
    //		holiday.Add("G. Aung San BD");
    //	} else if ((wd.getYear() >= 1969) && (wd.getMonth() == 2) && (wd.getDay() == 14)) {
    //		holiday.Add("Valentines Day");
    //	} else if ((wd.getYear() >= 1970) && (wd.getMonth() == 4) && (wd.getDay() == 22)) {
    //		holiday.Add("Earth Day");
    //	} else if ((wd.getYear() >= 1392) && (wd.getMonth() == 4) && (wd.getDay() == 1)) {
    //		holiday.Add("April Fools Day");
    //	} else if ((wd.getYear() >= 1948) && (wd.getMonth() == 5) && (wd.getDay() == 8)) {
    //		holiday.Add("Red Cross Day");
    //	} else if ((wd.getYear() >= 1994) && (wd.getMonth() == 10) && (wd.getDay() == 5)) {
    //		holiday.Add("World Teachers Day");
    //	} else if ((wd.getYear() >= 1947) && (wd.getMonth() == 10) && (wd.getDay() == 24)) {
    //		holiday.Add("United Nations Day");
    //	} else if ((wd.getYear() >= 1753) && (wd.getMonth() == 10) && (wd.getDay() == 31)) {
    //		holiday.Add("Halloween");
    //	}

    //	if ((wd.getYear() >= 1876) && (jd == doe)) {
    //		holiday.Add("Easter");
    //	} else if ((wd.getYear() >= 1876) && (jd == (doe - 2))) {
    //		holiday.Add("Good Friday");
    //	} else if (BinarySearchUtil.search(jd, ghEid2) >= 0) {
    //		holiday.Add("Eid");
    //	}
    //	if (BinarySearchUtil.search(jd, ghCNY) >= 0) {
    //		holiday.Add("Chinese New Year");
    //	}

    //	return holiday;
    //}

    /**
	 * Date of Easter using "Meeus/Jones/Butcher" algorithm Reference: Peter
	 * Duffett-Smith, Jonathan Zwart',
	 * "Practical Astronomy with your Calculator or Spreadsheet," 4th Etd,
	 * Cambridge university press, 2011. Page-4.
	 * 
	 * @param year
	 *            Western year
	 * @return julian day number dependency: w2j()
	 */
    private static double DoE(int year)
    {
        double a = year % 19;
        double b = Math.Floor((double)year / 100.0);
        double c = year % 100;
        double d = Math.Floor(b / 4.0);
        double e = b % 4;
        double f = Math.Floor((b + 8.0) / 25.0);
        double g = Math.Floor((b - f + 1.0) / 3.0);
        double h = (19 * a + b - d - g + 15) % 30;
        double i = Math.Floor(c / 4);
        double k = c % 4;
        double l = (32 + 2 * e + 2 * i - h - k) % 7;
        double m = Math.Floor((a + 11.0 * h + 22.0 * l) / 451.0);
        double q = h + l - 7 * m + 114;
        int day = (int)((q % 31) + 1);
        int month = (int)Math.Floor(q / 31.0);
        // this is for Gregorian
        return WesternDateKernel.w2j(year, month, day, 1, 0);
    }

    /**
	 * Myanmar Anniversary day
	 * 
	 * @param myear
	 *            Myanmar year
	 * @param mmonth
	 *            Myanmar month [Tagu=1, ... , Tabaung=12]
	 * @param monthDay
	 *            Month day [1 to 30]
	 * @param moonPhase
	 *            Moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon]
	 * @return List of holiday string
	 */
    public static List<string> mcd(double myear, int mmonth, int monthDay, int moonPhase)
    {

        List<string> holiday = new List<string>();

        if ((myear >= 1309) && (mmonth == 11) && (monthDay == 16))
        {
            holiday.Add("Mon National Day");
        } // the ancient founding of Hanthawady
        else if ((mmonth == 9) && (monthDay == 1))
        {
            holiday.Add("Shan New Year Day");
            if (myear >= 1306)
            {
                holiday.Add("Authors Day");
            }
        } // Nadaw waxing moon 1
        else if ((mmonth == 3) && (moonPhase == 1))
        {
            holiday.Add("Mahathamaya Day");
        } // Nayon full moon
        else if ((mmonth == 6) && (moonPhase == 1))
        {
            holiday.Add("Garudhamma Day");
        } // Tawthalin full moon
        else if ((myear >= 1356) && (mmonth == 10) && (moonPhase == 1))
        {
            holiday.Add("Mothers Day");
        } // Pyatho full moon
        else if ((myear >= 1370) && (mmonth == 12) && (moonPhase == 1))
        {
            holiday.Add("Fathers Day");
        } // Tabaung full moon
        else if ((mmonth == 5) && (moonPhase == 1))
        {
            holiday.Add("Metta Day");
            // if(my>=1324) {hs[h++]="Mon Revolution Day";}//Mon Revolution day
        } // Waguang full moon
        else if ((mmonth == 5) && (monthDay == 10))
        {
            holiday.Add("Taungpyone Pwe");
        } // Taung Pyone Pwe
        else if ((mmonth == 5) && (monthDay == 23))
        {
            holiday.Add("Yadanagu Pwe");
        } // Yadanagu Pwe

        // else if((my>=1119) && (mm==2) && (md==23))
        // {hs[h++]="Mon Fallen Day";}
        // else if((mm==12) && (md==12)) {hs[h++]="Mon Women Day";}

        return holiday;
    }


    /**
	 * 
	 * @param myanmarDate
	 *            MyanmarDate
	 * @return List of holiday string
	 */
    //public static List<string> getHoliday(MyanmarDate myanmarDate) {
    //	return getHoliday(myanmarDate, Config.get().getCalendarType());
    //}


    /**
	 * 
	 * @param myanmarDate
	 *            MyanmarDate
	 * @param calendarType
	 *            CalendarType
	 * @return List of holiday string
	 */
    //public static List<string> getHoliday(MyanmarDate myanmarDate, CalendarType calendarType) {

    //	WesternDate westernDate = WesternDateConverter.convert(myanmarDate.jd, calendarType);
    //	// Office Off
    //	List<string> hde = englishHoliday(westernDate.getYear(), westernDate.getMonth(), westernDate.getDay());
    //	List<string> hdm = myanmarHoliday(myanmarDate.myear, myanmarDate.mmonth, myanmarDate.monthDay,
    //			myanmarDate.moonPhase);
    //	List<string> hdt = thingyan(myanmarDate.jd, myanmarDate.myear, myanmarDate.monthType);
    //	List<string> hdo = otherHolidays(myanmarDate.jd); // Diwali Eid

    //	List<string> holiday = new List<string>();

    //	holiday.AddRange(hde);
    //	holiday.AddRange(hdm);
    //	holiday.AddRange(hdt);
    //	holiday.AddRange(hdo);

    //	return holiday;
    //}

    /**
	 * 
	 * @param myanmarDate
	 *            MyanmarDate object
	 * @return bool
	 */
    //public static bool isHoliday(MyanmarDate myanmarDate) {
    //	return getHoliday(myanmarDate).Count > 0 ? true : false;
    //}

    /**
	 * 
	 * @param holidayList
	 *            List Of Holiday
	 * @return bool
	 */
    public static bool isHoliday(List<string> holidayList)
    {
        return holidayList.Count > 0 ? true : false;
    }

    /**
	 * 
	 * @param myanmarDate
	 *            MyanmarDate Object
	 * @return List of holiday string
	 */
    //public static List<string> getAnniversary(MyanmarDate myanmarDate)
    //{
    //    return getAnniversary(myanmarDate, Config.get().getCalendarType());
    //}

    /**
	 * 
	 * @param myanmarDate
	 *            MyanmarDate
	 * @param calendarType
	 *            CalendarType
	 * @return List of holiday string
	 */
    //public static List<string> getAnniversary(MyanmarDate myanmarDate, CalendarType calendarType) {
    //	List<string> ecdf = ecd(myanmarDate.jd, calendarType); // anniversary day
    //	List<string> mcdf = mcd(myanmarDate.myear, myanmarDate.mmonth, myanmarDate.monthDay, myanmarDate.moonPhase);

    //	List<string> holiday = new List<string>();

    //	holiday.AddRange(ecdf);
    //	holiday.AddRange(mcdf);

    //	return holiday;
    //}

}
