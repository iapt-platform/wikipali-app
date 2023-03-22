//package mmcalendar;

//import java.io.Serializable;


using System;
using System.Text;
/**
* Myanmar Date 
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0.2
* 
* @since 1.0
*
*/
public class MyanmarDate
{

    public static /*final*/ long serialVersionUID = 1L;

    public static /*final*/ string[] MMA = { "First Waso", "Tagu", "Kason", "Nayon", "Waso", "Wagaung", "Tawthalin", "Thadingyut",
            "Tazaungmon", "Nadaw", "Pyatho", "Tabodwe", "Tabaung" };

    /**
	 * New Moon mean Dark moon
	 */
    public static /*final*/ string[] MSA = { "waxing", "full moon", "waning", "new moon" };

    /**
	 * Week Days
	 */
    public static /*final*/ string[] WDA = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

    /**
	 * Myanmar year
	 */
    public int myear;

    /**
	 * year type [0=common, 1=little watat, 2=big watat]
	 */
    public int yearType;

    /**
	 * myl : year length [normal = 354, small watat = 384, or big watat = 385 days]
	 */
    public int yearLength;

    /**
	 * mm :
	 * month Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd) Waso=4, Wagaung=5,
	 * Tawthalin=6, Thadingyut=7, Tazaungmon=8, Nadaw=9, Pyatho=10, Tabodwe=11,
	 * Tabaung=12
	 */
    public int mmonth;

    /**
	 * mmt: month type [1=hnaung, 0= Oo]
	 */
    public int monthType;

    /**
	 * mml: month length [29 or 30 days]
	 */
    public int monthLength;

    /**
	 * md: month day [1 to 30]
	 */
    public int monthDay;

    /**
	 * fd: fortnight day [1 to 15],
	 */
    public int fortnightDay;

    /**
	 * 
	 * mp :moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon],
	 */
    public int moonPhase;

    /**
	 * wd: week day [0=sat, 1=sun, ..., 6=fri]
	 */
    public int weekDay;

    /**
	 * julian day number
	 */
    public double jd;

    public MyanmarDate()
    {
    }

    /**
	 * 
	 * @param myear
	 *            Myanmar year
	 * @param mmonth
	 *            month Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd) Waso=4,
	 *            Wagaung=5, Tawthalin=6, Thadingyut=7, Tazaungmon=8, Nadaw=9,
	 *            Pyatho=10, Tabodwe=11, Tabaung=12
	 * @param mmt
	 *            month type [1=hnaung, 0= Oo]
	 * @param fd
	 *            fortnight day [1 to 15],
	 * @param moonPhase
	 *            moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon],
	 */
    public MyanmarDate(int myear, int mmonth, int mmt, int fd, int moonPhase)
    {

        //if (myear < 0){
        //	throw new IllegalArgumentException("Myanmar year must be positive number");
        //}
        //if (mmonth < 0 || mmonth > 13){
        //	throw new IllegalArgumentException("Month must be 0 to 12");
        //}
        //if (mmt < 0 || mmt > 1){
        //	throw new IllegalArgumentException("Month type must be 0 to 1");
        //}
        //if (fd < 1 || fd > 15){
        //	throw new IllegalArgumentException("Fortnight day must be 0 to 15");
        //}
        //if (moonPhase < 0 || moonPhase > 3){
        //	throw new IllegalArgumentException("Moon phase must be 0 to 3");
        //}

        this.myear = myear;
        this.mmonth = mmonth;
        this.monthType = mmt;
        this.fortnightDay = fd;
        this.moonPhase = moonPhase;
    }
    public int getBuddhistEraInt()
    {
        return myear + 1182;
    }
    public string getBuddhistEra(LanguageCatalog languageCatalog)
    {
        return NumberToStringUtil.convert(myear + 1182, languageCatalog);
    }

    public string getBuddhistEra()
    {
        return getBuddhistEra(new LanguageCatalog());
    }

    public string getYear(LanguageCatalog languageCatalog)
    {
        return NumberToStringUtil.convert(myear, languageCatalog);
    }

    public string getYear()
    {
        return getYear(new LanguageCatalog());
    }

    public int getYearInt()
    {
        return myear;
    }

    /**
	 * 
	 * @return 0=common, 1=little watat, 2=big watat
	 */
    public int getYearType()
    {
        /* 0=common, 1=little watat, 2=big watat */
        return yearType;
    }

    public string getMnt()
    {
        return getMnt(LanguageCatalog.getInstance());
    }

    public string getMnt(LanguageCatalog languageCatalog)
    {
        /* 0=common, 1=little watat, 2=big watat */
        StringBuilder stringBuilder = new StringBuilder();
        if (monthType > 0)
        {
            stringBuilder.Append(languageCatalog.translate("Late"));
        }

        if (yearType > 0 && mmonth == 4)
        {
            stringBuilder.Append(languageCatalog.translate("Second"));
        }

        return stringBuilder.ToString();
    }

    /**
	 * Myanmar Year Length
	 * @return [normal=354, small watat=384, big watat=385]
	 */
    public int getYearLength()
    {
        return yearLength;
    }

    public string getMonthName()
    {
        return getMonthName(LanguageCatalog.getInstance());
    }

    public string getMonthName(LanguageCatalog languageCatalog)
    {
        return getMnt() + languageCatalog.translate(MMA[this.mmonth]);
    }

    /**
	 * @return int Tagu=1, Kason=2, Nayon=3, 1st Waso=0, (2nd) Waso=4,
	 *         Wagaung=5, * Tawthalin=6, Thadingyut=7, Tazaungmon=8, Nadaw=9,
	 *         Pyatho=10, Tabodwe=11, * Tabaung=12
	 */
    public int getMonth()
    {
        return mmonth;
    }

    /**
	 * moon phase 
	 * @return [0=waxing, 1=full moon, 2=waning, 3=new moon]
	 */
    public int getMoonPhraseInt()
    {
        return moonPhase;
    }

    public string getMoonPhase()
    {
        return getMoonPhase(LanguageCatalog.getInstance());
    }

    public string getMoonPhase(LanguageCatalog languageCatalog)
    {
        return languageCatalog.translate(MSA[this.moonPhase]);
    }

    /**
	 * 
	 * @return month length [29 or 30 days]
	 */
    public int getMonthLength()
    {
        return monthLength;
    }

    /**
	 * 
	 * @return month day [1 to 30]
	 */
    public int getMonthDay()
    {
        return monthDay;
    }

    /**
	 * 
	 * @return month type [1 = hnaung, 0 = Oo]
	 */
    public int getMonthType()
    {
        return monthType;
    }

    /**
	 * 
	 * @return fortnight day [1 to 15],
	 */
    public int getFortnightDayInt()
    {
        return fortnightDay;
    }

    public string getFortnightDay()
    {
        return getFortnightDay(LanguageCatalog.getInstance());
    }

    public string getFortnightDay(LanguageCatalog languageCatalog)
    {
        return ((moonPhase % 2) == 0) ? NumberToStringUtil.convert(fortnightDay, languageCatalog) : "";
    }

    /**
	 * 
	 * @return week day [0=sat, 1=sun, ..., 6=fri]
	 */
    public int getWeekDayInt()
    {
        return weekDay;
    }

    public string getWeekDay()
    {
        return getWeekDay(LanguageCatalog.getInstance());
    }

    public string getWeekDay(LanguageCatalog languageCatalog)
    {
        return languageCatalog.translate(WDA[this.weekDay]);
    }

    /**
	 * 
	 * @return julian day number
	 */
    public double getJulianDayNumber()
    {
        return jd;
    }

    /**
	 * 
	 * @return moon phase [0=waxing, 1=full moon, 2=waning, 3=new moon],
	 */
    public int getMoonPhrase()
    {
        return moonPhase;
    }

    public bool isWeekend()
    {
        return ((weekDay == 0) || (weekDay == 1)) ? true : false;
    }

    /**
     * @param pattern the pattern describing the date and time format
	 * @exception NullPointerException if the given pattern is null
	 */
    public string format(string pattern)
    {
        return format(pattern, LanguageCatalog.getInstance());
    }

    /**
     * 
     * @param pattern
     *      the pattern describing the date and time format
     * @param languageCatalog
     * @exception NullPointerException if the given pattern is null return string
     * @return
     */
    public string format(string pattern, LanguageCatalog languageCatalog)
    {

        if (pattern == null || languageCatalog == null)
        {
            return "";
            //throw new NullPointerException();
        }

        char[] charArray = pattern.ToCharArray();

        StringBuilder stringBuilder = new StringBuilder();

        for (int index = 0; index < charArray.Length; index++)
        {
            switch (charArray[index])
            {
                case MyanmarDateFormat.SASANA_YEAR:
                    stringBuilder.Append(languageCatalog.translate("Sasana Year"));
                    break;
                case MyanmarDateFormat.BUDDHIST_ERA:
                    stringBuilder.Append(getBuddhistEra(languageCatalog));
                    break;
                case MyanmarDateFormat.BURMESE_YEAR:
                    stringBuilder.Append(languageCatalog.translate("Myanmar Year"));
                    break;
                case MyanmarDateFormat.MYANMAR_YEAR:
                    stringBuilder.Append(getYear(languageCatalog));
                    break;
                case MyanmarDateFormat.KU:
                    stringBuilder.Append(languageCatalog.translate("Ku"));
                    break;
                case MyanmarDateFormat.MONTH_IN_YEAR:
                    stringBuilder.Append(getMonthName(languageCatalog));
                    break;
                case MyanmarDateFormat.MOON_PHASE:
                    stringBuilder.Append(getMoonPhase(languageCatalog));
                    break;
                case MyanmarDateFormat.FORTNIGHT_DAY:
                    stringBuilder.Append(getFortnightDay(languageCatalog));
                    break;
                case MyanmarDateFormat.DAY_NAME_IN_WEEK:
                    stringBuilder.Append(getWeekDay(languageCatalog));
                    break;
                case MyanmarDateFormat.NAY:
                    stringBuilder.Append(languageCatalog.translate("Nay"));
                    break;
                case MyanmarDateFormat.YAT:
                    stringBuilder.Append(languageCatalog.translate("Yat"));
                    break;
                default:
                    stringBuilder.Append(charArray[index]);
                    break;
            }
        }
        return stringBuilder.ToString();
    }

    //@Override
    public string toString()
    {
        LanguageCatalog languageCatalog = LanguageCatalog.getInstance();
        return toString(languageCatalog);
    }

    public string toString(LanguageCatalog languageCatalog)
    {
        return format(Config.SIMPLE_MYANMAR_DATE_FORMAT_PATTERN, languageCatalog);
    }

    //@Override
    public int compareTo(MyanmarDate anotherMyanmarDate)
    {
        //?????????????
        //return Double.Compare(jd, anotherMyanmarDate.jd);
        return jd.CompareTo(anotherMyanmarDate.jd);
    }

    //@Override
    //public int hashCode() {
    //	/*final*/ int prime = 31;
    //	int result = 1;
    //	result = prime * result + fortnightDay;
    //	long temp;
    //	temp = Double.DoubleToLongBits(jd);
    //	result = prime * result + (int) (temp ^ (temp >>> 32));
    //	result = prime * result + mmonth;
    //	result = prime * result + monthDay;
    //	result = prime * result + monthLength;
    //	result = prime * result + monthType;
    //	result = prime * result + moonPhase;
    //	result = prime * result + myear;
    //	result = prime * result + weekDay;
    //	result = prime * result + yearLength;
    //	result = prime * result + yearType;
    //	return result;
    //}

    //@Override
    //public bool equals(Object obj) {
    //	if (this == obj)
    //		return true;
    //	if (obj == null)
    //		return false;
    //	if (getClass() != obj.getClass())
    //		return false;
    //	MyanmarDate other = (MyanmarDate) obj;
    //	if (fortnightDay != other.fortnightDay)
    //		return false;
    //	if (Double.doubleToLongBits(jd) != Double.doubleToLongBits(other.jd))
    //		return false;
    //	if (mmonth != other.mmonth)
    //		return false;
    //	if (monthDay != other.monthDay)
    //		return false;
    //	if (monthLength != other.monthLength)
    //		return false;
    //	if (monthType != other.monthType)
    //		return false;
    //	if (moonPhase != other.moonPhase)
    //		return false;
    //	if (myear != other.myear)
    //		return false;
    //	if (weekDay != other.weekDay)
    //		return false;
    //	if (yearLength != other.yearLength)
    //		return false;
    //	if (yearType != other.yearType)
    //		return false;
    //	return true;
    //}
}
