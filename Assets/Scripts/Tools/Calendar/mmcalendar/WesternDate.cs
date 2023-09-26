//package mmcalendar;

//import java.io.Serializable;

/**
 * Western Date
 * 
 * @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
 * 
 * @version 1.0.2
 * 
 * @since 1.0
 *
 */
public class WesternDate
{

    private static /*final*/ long serialVersionUID = -198088735719287260L;

    private int year;
    private int month;
    private int day;
    private int hour;
    private int minute;
    private int second;

    public WesternDate(int year, int month, int day, int hour, int minute, int second)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }

    public int getYear()
    {
        return year;
    }

    public int getMonth()
    {
        return month;
    }

    public int getDay()
    {
        return day;
    }

    public int getHour()
    {
        return hour;
    }

    public int getMinute()
    {
        return minute;
    }

    public int getSecond()
    {
        return second;
    }

    //@Override	
    public string toString()
    {
        return "WesternDate [year=" + year + ", month=" + month + ", day=" + day + ", hour=" + hour + ", minute="
                + minute + ", second=" + second + "]";
    }

    //@Override
    public int hashCode()
    {
        int prime = 31;
        int result = 1;
        result = prime * result + day;
        result = prime * result + hour;
        result = prime * result + minute;
        result = prime * result + month;
        result = prime * result + second;
        result = prime * result + year;
        return result;
    }

    //@Override
    public bool equals(object obj)
    {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        WesternDate other = (WesternDate)obj;
        if (day != other.day)
            return false;
        if (hour != other.hour)
            return false;
        if (minute != other.minute)
            return false;
        if (month != other.month)
            return false;
        if (second != other.second)
            return false;
        if (year != other.year)
            return false;
        return true;
    }

}
