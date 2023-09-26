//package mmcalendar;

//import java.util.ArrayList;
//import java.util.List;


/*final*/ 
using System.Collections.Generic;
/**
* Era
* 
* @author <a href="mailto:chanmratekoko.dev@gmail.com">Chan Mrate Ko Ko</a>
* 
* @version 1.0.1
* @since 1.0
*
*/
class Era {
    //private Era() { }
    private static Era manager = null;
    //静态工厂方法 
    public static Era Instance()
    {
        if (manager == null)
        {
            manager = new Era();
            manager.Init();
        }
        return manager;
    }
    /**
	 * era id
	 */
    public double eid;

    /**
	 * beginning Myanmar year
	 */
    public int begin;

    /**
	 * ending Myanmar year
	 */
    public int end;

    /**
	 * watat offset to compensate
	 */
    public double WO;

    /**
	 * number of months to find excess days
	 */
    public int NM;

    /**
	 * exceptions for full moon days
	 */
    public int[][] fme;

    /**
	 * exceptions for watat years
	 */
    public int[][] wte;

	public /*final*/ List<Era> G_ERAS = new List<Era>();

	//static {
    public void Init(){
        // The first era (the era of Myanmar kings: ME1216 and before)
        // Makaranta system 1 (ME 0 - 797)
        Era era_1_1 = new Era();
        era_1_1.eid = 1.1f;
        era_1_1.begin = -999;
        era_1_1.end = 797;
        era_1_1.WO = -1.1;
        era_1_1.NM = -1;
        era_1_1.fme = new int[][] { new int[]{ 205, 1 },new int[] { 246, 1 }, new int[]{ 471, 1 },new int[] { 572, -1 }, new int[]{ 651, 1 },
            new int[]{ 653, 2 },new int[] { 656, 1 },  new int[]{ 672, 1 }, new int[]{ 729, 1 }, new int[]{ 767, -1 } };
        // reference
        era_1_1.wte = new int[][] { }; // exceptions for watat years

        G_ERAS.Add(era_1_1);

        // Makaranta system 2 (ME 798 - 1099)
        Era era_1_2 = new Era();
        era_1_2.eid = 1.2;
        era_1_2.begin = 798;
        era_1_2.end = 1099;
        era_1_2.WO = -1.1;
        era_1_2.NM = -1;
        era_1_2.fme = new int[][] { new int[]{ 813, -1 }, new int[]{ 849, -1 }, new int[]{ 851, -1 },new int[] { 854, -1 },new int[] { 927, -1 },new int[] { 933, -1 },
            new int[]    { 936, -1 }, new int[]{ 938, -1 }, new int[]{ 949, -1 }, new int[]{ 952, -1 }, new int[]{ 963, -1 },new int[] { 968, -1 }, new int[]{ 1039, -1 } };
        // as reference
        era_1_2.wte = new int[][] { };
        G_ERAS.Add(era_1_2);

        // Thandeikta (ME 1100 - 1216)
        Era era_1_3 = new Era();
        era_1_3.eid = 1.3;
        era_1_3.begin = 1100;
        era_1_3.end = 1216;
        era_1_3.WO = -0.85;
        era_1_3.NM = -1;
        era_1_3.fme = new int[][] { new int[] { 1201, 1 }, new int[] { 1202, 0 } };
        G_ERAS.Add(era_1_3);

        // ---------------------------------------------------------
        // The second era (the era under British colony: 1217 ME - 1311 ME)
        Era era_2 = new Era();
        era_2.eid = 2;
        era_2.begin = 1217;
        era_2.end = 1311;
        era_2.WO = -1;
        era_2.NM = 4;
        era_2.fme = new int[][] { new int[] { 1234, 1 }, new int[] { 1261, -1 } };
        era_2.wte = new int[][] { new int[] { 1263, 1 }, new int[]  { 1264, 0 } };
        G_ERAS.Add(era_2);

        // ---------------------------------------------------------
        // The third era (the era after Independence 1312 ME and after)
        Era era_3 = new Era();
        era_3.eid = 3;
        era_3.begin = 1312;
        era_3.end = 9999;
        era_3.WO = -0.5;
        era_3.NM = 8;
        era_3.fme = new int[][] { new int[] { 1377, 1 } };
        era_3.wte = new int[][] { new int[] { 1344, 1 }, new int[] { 1345, 0 } };
        G_ERAS.Add(era_3);
    }
	//}

    /**
     * Don't let anyone instantiate this class.
     */

}
