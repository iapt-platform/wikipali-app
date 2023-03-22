//package mmcalendar;

//import java.util.ArrayList;
//import java.util.HashMap;
//import java.util.List;
//import java.util.Map;


using System;
using System.Collections.Generic;
/**
* Core Calculation and Algorithms for Myanmar Date
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0.2
* 
*/
public /*final*/ class MyanmarDateKernel
{

    static /*final*/ string[] EMA = { "First Waso", "Tagu", "Kason", "Nayon", "Waso", "Wagaung", "Tawthalin", "Thadingyut",
            "Tazaungmon", "Nadaw", "Pyatho", "Tabodwe", "Tabaung", "Late Tagu", "Late Kason" };

    /**
     * Don't let anyone instantiate this class.
     */
    private MyanmarDateKernel()
    {
    }

    /**
	 * Julian date to Myanmar date dependency: chk_my(my)
	 * 
	 * @param jd
	 *            : julian date
	 * @return {@link MyanmarDate} Object
	 * 			MyanmarDate year year type [0=common, 1=little watat, 2=big
	 *         watat], month [Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd)
	 *         Waso=4, Wagaung=5, Tawthalin=6, Thadingyut=7, Tazaungmon=8,
	 *         Nadaw=9, Pyatho=10, Tabodwe=11, Tabaung=12 ] month type
	 *         [1=hnaung, 0= Oo] month length [29 or 30 days] month day [1 to
	 *         30] fortnight day [1 to 15] moon phase [0=waxing, 1=full moon,
	 *         2=waning, 3=new moon] week day [0=sat, 1=sun, ..., 6=fri]
	 */
    public static MyanmarDate j2m(double jd)
    {

        double jdn, dd, yearLength, monthType, a, b, c, e, f, mmonth, monthDay, monthLength, moonPhase, moonPhaseD, fortnightDay, weekDay;

        double myear;

        Dictionary<string, Double> yo;

        // convert jd to jdn
        jdn = Math.Round(jd);
        /// Myanmar year
        myear = Math.Floor((jdn - 0.5 - Constants2.MO) / Constants2.SY);
        // check year
        yo = chk_my(myear);
        // day count
        dd = jdn - yo["tg1"] + 1;
        b = Math.Floor(yo["myt"] / 2.0);
        // big wa and common yr
        c = Math.Floor(1 / (yo["myt"] + 1));
        // year length
        yearLength = 354 + (1 - c) * 30 + b;
        // month type: Hnaung =1 or Oo = 0
        monthType = Math.Floor((dd - 1) / yearLength);
        dd -= monthType * yearLength;
        // adjust day count and threshold
        a = Math.Floor((dd + 423) / 512.0);
        // month
        mmonth = Math.Floor((dd - b * a + c * a * 30.0 + 29.26) / 29.544);
        e = Math.Floor((mmonth + 12) / 16.0);
        f = Math.Floor((mmonth + 11) / 16.0);
        // day
        monthDay = dd - Math.Floor(29.544 * mmonth - 29.26) - b * e + c * f * 30;
        mmonth += f * 3 - e * 4;
        // adjust month and month length
        monthLength = 30 - mmonth % 2;

        if (mmonth == 3)
        {
            // adjust if Nayon in big watat
            monthLength += b;
        }
        //????????????????????????????
        if ((int)mmonth == 0)
            mmonth -= f * 3 - e * 4;
        //;
        moonPhase = Math.Floor((monthDay + 1) / 16.0) + Math.Floor(monthDay / 16.0) + Math.Floor(monthDay / monthLength);
        // waxing or waning day
        fortnightDay = monthDay - 15 * Math.Floor(monthDay / 16.0);
        // week day
        weekDay = (jdn + 2) % 7;

        MyanmarDate myanmarDate = new MyanmarDate();
        myanmarDate.myear = (int)myear;
        //??????????????????
        myanmarDate.yearType = (int)yo["myt"]; //.intValue();
        myanmarDate.yearLength = (int)yearLength;
        myanmarDate.mmonth = (int)mmonth;
        myanmarDate.monthType = (int)monthType;
        myanmarDate.monthLength = (int)monthLength;
        myanmarDate.monthDay = (int)monthDay;
        myanmarDate.moonPhase = (int)moonPhase;
        myanmarDate.fortnightDay = (int)fortnightDay;
        myanmarDate.weekDay = (int)weekDay;
        myanmarDate.jd = jd;

        return myanmarDate;
    }

    /**
	 * Check Myanmar Year dependency: chk_watat(my)
	 * 
	 * @param myear
	 *            myanmar year
	 * @return 	map 
	 * 			myt :year type [0=common, 1=little watat, 2=big watat] 
	 * 			tg1 :the 1st day of Tagu as Julian Day Number 
	 * 			fm : full moon day of [2nd] Waso as Julain Day Number) 
	 * 			werr: watat error 0 or 1
	 * 
	 */
    //满月日
    public static Dictionary<string, Double> chk_my(double myear)
    {

        int yd = 0;
        Dictionary<string, Double> y1;
        double nd = 0;
        double werr = 0;
        double fm = 0;

        Dictionary<string, Double> y2 = chk_watat(myear);
        double myt = y2["watat"];

        do
        {
            yd++;
            y1 = chk_watat(myear - yd);
        } while (y1["watat"] == 0 && yd < 3);

        if (myt > 0)
        {
            nd = (y2["fm"] - y1["fm"]) % 354;
            myt = Math.Floor(nd / 31.0) + 1;
            fm = y2["fm"];
            if (nd != 30 && nd != 31)
            {
                werr = 1;
            }
        }
        else
        {
            fm = y1["fm"] + 354 * yd;
        }

        double tg1 = y1["fm"] + 354 * yd - 102;

        // return {myt:myt,tg1:tg1,fm:fm,werr:werr};

        Dictionary<string, Double> map = new Dictionary<string, Double>();
        map.Add("myt", myt);
        map.Add("tg1", tg1);
        map.Add("fm", fm);
        map.Add("werr", werr);

        return map;
    }

    /**
	 * Check watat (intercalary month) dependency: chk_exception(my,fm,watat,ei)
	 * 
	 * @param myear
	 *            myanmar year
	 * @return map watat - intercalary month [1=watat, 0=common] 
	 * 			fm - full moon day of 2nd Waso in jdn [valid for watat years only])
	 */
    //满月日
    private static Dictionary<string, Double> chk_watat(double myear)
    {

        int i = Era.Instance().G_ERAS.Count - 1;

        do
        {
            // get data for respective era
            if (myear >= Era.Instance().G_ERAS[i].begin)
            {
                break;
            }
            i--;
        } while (i > 0);

        Era era = Era.Instance().G_ERAS[i];
        int NM = era.NM;
        double WO = era.WO;

        // threshold to adjust
        double TA = (Constants2.SY / 12.0 - Constants2.LM) * (12 - NM);
        double ed = (Constants2.SY * (myear + 3739)) % Constants2.LM; // excess day

        if (ed < TA)
        {
            // adjust excess days
            ed += Constants2.LM;
        }

        /* full moon day of 2nd Waso */
        double fm = Math.Round(Constants2.SY * myear + Constants2.MO - ed + 4.5 * Constants2.LM + WO);

        double TW = 0;
        double watat = 0; // find watat

        if (era.eid >= 2)
        {
            /* if 2nd era or later find watat based on excess days */
            TW = Constants2.LM - (Constants2.SY / 12.0 - Constants2.LM) * NM;
            if (ed >= TW)
            {
                watat = 1;
            }
        }
        else
        {
            /* if 1st era,find watat by 19 years metonic cycle */
            /* Myanmar year is divided by 19 and there is intercalary month */
            /* if the remainder is 2,5,7,10,13,15,18 */
            /*
			 * https://github.com/kanasimi/CeJS/blob/master/data/date/calendar.
			 * js#L2330
			 */
            watat = (myear * 7 + 2) % 19;
            if (watat < 0)
            {
                watat += 19;
            }
            watat = Math.Floor(watat / 12.0);
        }

        i = BinarySearchUtil.search(myear, era.wte);
        if (i >= 0)
        {
            // correct watat exceptions
            watat = era.wte[i][1];
        }

        if (watat > 0)
        {
            i = BinarySearchUtil.search(myear, era.fme);
            if (i >= 0)
            {
                fm += era.fme[i][1];
            }
        } // correct full moon day exceptions

        Dictionary<string, Double> map = new Dictionary<string, Double>();
        map.Add("fm", fm);
        map.Add("watat", watat);
        // return {fm:fm,watat:watat};

        return map;
    }

    /**
	 * Myanmar date to Julian date
	 * @param myanmarDate {@link MyanmarDate} Object
	 * @return julian day number
	 */
    public static double m2j(MyanmarDate myanmarDate)
    {
        return m2j(myanmarDate.myear, myanmarDate.mmonth, myanmarDate.monthType, myanmarDate.moonPhase, myanmarDate.fortnightDay);
    }


    /**
	 * Myanmar date to Julian date dependency: chk_my(my)
	 * 
	 * @param myear
	 *            Myanmar Year
	 * @param mmonth
	 *            Myanmar month [Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd)
	 *            Waso=4, Wagaung=5, Tawthalin=6, Thadingyut=7, Tazaungmon=8,
	 *            Nadaw=9, Pyatho=10, Tabodwe=11, Tabaung=12 ]
	 * @param monthType
	 *            month type [1=hnaung, 0=Oo]
	 * @param moonPhase
	 *            moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon]
	 * @param fortnightDay
	 *            fortnight day [1 to 15]
	 * @return julian day number
	 */
    public static double m2j(double myear, double mmonth, double monthType, double moonPhase, double fortnightDay)
    {

        double b, c, mml, m1, m2, md, dd;

        //check year
        Dictionary<string, Double> yo = chk_my(myear);

        b = Math.Floor(yo["myt"] / 2);

        //if big watat and common year
        c = (yo["myt"] == 0) ? 1 : 0;

        //month length
        mml = 30 - mmonth % 2;

        if (mmonth == 3)
        {
            //adjust if Nayon in big watat
            mml += b;
        }

        m1 = moonPhase % 2;
        m2 = Math.Floor(moonPhase / 2.0);
        md = m1 * (15 + m2 * (mml - 15)) + (1 - m1) * (fortnightDay + 15 * m2);

        // adjust month
        mmonth += 4 - Math.Floor((mmonth + 15) / 16.0) * 4 + Math.Floor((mmonth + 12) / 16);


        dd = md + Math.Floor(29.544 * mmonth - 29.26) - c * Math.Floor((mmonth + 11) / 16.0) * 30
                + b * Math.Floor((mmonth + 12) / 16.0);
        // year length
        double myl = 354 + (1 - c) * 30 + b;
        // adjust day count
        dd += monthType * myl;

        return dd + yo["tg1"] - 1;
    }

    /**
     * Myanmar date to Julian date dependency: chk_my(my)
     * 
     * @param myear
     *            Myanmar Year
     * @param mmonth
     *            Myanmar month [Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd)
     *            Waso=4, Wagaung=5, Tawthalin=6, Thadingyut=7, Tazaungmon=8,
     *            Nadaw=9, Pyatho=10, Tabodwe=11, Tabaung=12 , Late Tagu = 13, Late Kason = 14]
     * @param mmday
     *            Myanmar day [1 to 29 or 30]
     * @return julian day number
     */
    public static double m2j(double myear, double mmonth, double mmday)
    {
        double monthType = Math.Floor(mmonth / 13);
        double month = mmonth % 12;
        return m2j(myear, month, monthType, 0, mmday);
    }

    /**
	 * Time to Fraction of day starting from 12 noon (t2d)
	 * 
	 * @param hour
	 *            Hour
	 * @param minute
	 *            Minute
	 * @param second
	 *            Second
	 * @return fraction of day
	 */
    static double time2DayFraction(double hour, double minute, double second)
    {
        return ((hour - 12) / 24) + (minute / 1440) + (second / 86400);
    }

    /**
	 * Calculate Myanmar Month List
	 * 
	 * @param myear
	 *            Myanmar Year
	 * @param mmonth
	 *            Myanmar month [Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd)
	 *            Waso=4, Wagaung=5, Tawthalin=6, Thadingyut=7, Tazaungmon=8,
	 *            Nadaw=9, Pyatho=10, Tabodwe=11, Tabaung=12 ]
	 * @return {@link MyanmarMonths} Object
	 */
    public static MyanmarMonths getMyanmarMonth(int myear, int mmonth)
    {

        double j1 = Math.Round(Constants2.SY * myear + Constants2.MO) + 1;
        double j2 = Math.Round(Constants2.SY * (myear + 1) + Constants2.MO);

        MyanmarDate M1 = j2m(j1);
        MyanmarDate M2 = j2m(j2);

        int si = M1.mmonth + 12 * M1.monthType;
        int ei = M2.mmonth + 12 * M2.monthType;

        if (si == 0)
        {
            si = 4;
        }
        if (mmonth == 0 && M1.yearType == 0)
        {
            mmonth = 4;
        }
        if (mmonth != 0 && mmonth < si)
        {
            mmonth = si;
        }
        if (mmonth > ei)
        {
            mmonth = ei;
        }

        List<int> index = new List<int>();
        List<string> monthNameList = new List<string>();
        int currentIndex = 0;

        for (int i = si; i <= ei; i++)
        {
            if (i == 4 && M1.yearType != 0)
            {
                index.Add(0);
                monthNameList.Add(EMA[0]);
                if (mmonth == 0)
                {
                    currentIndex = 0;
                }
            }
            index.Add(i);
            monthNameList.Add(((i == 4 && M1.yearType != 0) ? "Second " : "") + EMA[i]);

            if (i == mmonth)
            {
                //if(Mathf.Abs(i - mmonth) < 0.0000001 ) {
                currentIndex = i;
            }
        }

        return new MyanmarMonths(index, monthNameList, currentIndex);
    }
}
