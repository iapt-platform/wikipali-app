//package mmcalendar;


using System;
using UnityEngine;
/**
* Number To Specific Language string
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0
*
*/
public /*final*/ class NumberToStringUtil {
	
    /**
     * Don't let anyone instantiate this class.
     */
	private NumberToStringUtil() {
	}

	/**
	 * Number to string
	 * @param number Number 
	 * @param languageCatalog LanguageCatalog object
	 * @return string
	 */
	public static string convert(double number, LanguageCatalog languageCatalog) {
		int r = 0;
		string s = "", g = "";
		if (number < 0) {
			g = "-";
			number = Math.Abs(number);
		}
		number = Math.Floor(number);
		do {
			r = (int) (number % 10);
			number = Math.Floor(number / 10.0);
			s = languageCatalog.translate(r.ToString()) + s;
		} while (number > 0);
		return (g + s);
	}

}
