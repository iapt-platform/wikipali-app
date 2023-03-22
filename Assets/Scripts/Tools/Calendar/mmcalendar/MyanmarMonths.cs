//package mmcalendar;

//import java.util.ArrayList;
//import java.util.List;


using System.Collections.Generic;
/**
* Myanmar Months List for Specific Myanmar Year
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0.2 
* @since 1.0.1
*
*/
public class MyanmarMonths {

	private List<int> index = new List<int>();
	private List<string> monthNameList = new List<string>();
	private int currentIndex; //Calculation Month

	public MyanmarMonths(List<int> index, List<string> monthNameList, int currentIndex) {
		this.index = new List<int>(index);
		this.monthNameList = new List<string>(monthNameList);
		this.currentIndex = currentIndex;
	}

	public List<int> getIndex() {
		return index;
	}

	public List<string> getMonthNameList() {
		return monthNameList;
	}

	public List<string> getMonthNameList(LanguageCatalog languageCatalog) {
        //???????
		//if (languageCatalog.getLanguage() == Language.ENGLISH) {
		//	return monthNameList;
		//}
		List<string> temp = new List<string>();
		foreach (string s in monthNameList) {
			temp.Add(languageCatalog.translate(s));
		}
		return temp;
	}

	public int getCurrentIndex() {
		return currentIndex;
	}

}
