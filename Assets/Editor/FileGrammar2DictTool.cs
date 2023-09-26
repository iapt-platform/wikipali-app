using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
public class FileGrammar2DictTool
{
    //Dictionary<string, string[]> test = new Dictionary<string, string[]>(){
    //    {"1",  new string[]{
    //        "1111\n",
    //        "1111\n",
    //        "1111\n",
    //        "1111\n",
    //        "1111\n",
    //    }},

    //};
    //去掉chapter文件title变量 换行
    [MenuItem("Assets/Tools/ProcessGrammerFiles")]
    public static void ProcessGrammerFiles()
    {
        Dictionary<string, string[]> res = new Dictionary<string, string[]>();
        //支持多选
        string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
        for (int j = 0; j < guids.Length; j++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);//通过GUID获取路径
                                                                       //TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

            string[] textTxt = File.ReadAllLines(assetPath);
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            res.Add(fileNameWithoutExtension, textTxt);


            //File.WriteAllLines(assetPath, res.ToArray());
            //File.WriteAllLines("Assets/Editor/chapter - bak_P.csv", res.ToArray());
        }
        List<string> resText = new List<string>();
        //resText.Add(" Dictionary<string, string[]> grammer_cn = new Dictionary<string, string[]>(){");
        //resText.Add(" Dictionary<string, string[]> grammer_en = new Dictionary<string, string[]>(){");
        resText.Add(" Dictionary<string, string[]> grammer_tw = new Dictionary<string, string[]>(){");
        foreach (KeyValuePair<string, string[]> kvp in res)
        {
            string a = string.Format("{0} {1} {2}", 1, 2, 3);
            //resText.Add(string.Format("  {\" {0}   \",  new string[]{", kvp.Key));
            resText.Add("  {\"" + kvp.Key + "\",  new string[]{");
            for (int i = 0; i < kvp.Value.Length; i++)
            {

                string temp = kvp.Value[i].Replace("\\", "\\\\");
                temp = temp.Replace("\"", "\\\"");
                resText.Add("\"" + temp + "\",");
            }

            resText.Add("}},");

        }
        resText.Add("};");
        //File.WriteAllLines("Assets/Editor/grammer_CN.txt", resText.ToArray());
        //File.WriteAllLines("Assets/Editor/grammer_EN.txt", resText.ToArray());
        File.WriteAllLines("Assets/Editor/grammer_TW.txt", resText.ToArray());
    }
    //处理 grm_abbr
    [MenuItem("Assets/Tools/ProcessGrm_AbbrFiles")]
    public static void ProcessGrmAbbrFiles()
    {
        Dictionary<string, string[]> res = new Dictionary<string, string[]>();
        //支持多选
        string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
                                                                   //TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

        string[] textTxt = File.ReadAllLines(assetPath);
        List<string> sb = new List<string>();
        List<string> repeat = new List<string>();
        int l = textTxt.Length;
        for (int i = 0; i < l; i++)
        {
            string resI = "{";
            string[] temp = textTxt[i].Split(',');
            if (temp.Length < 3)
            {
                continue;
            }
            string[] temp1 = temp[1].Split('>');
            if (repeat.Contains(temp1[1]))
            {
                continue;
            }
            repeat.Add(temp1[1]);
            resI += temp1[1] + ",";
            string[] temp2 = temp[2].Split('>');
            resI += temp2[1].Replace("\"]", "\"},");
            sb.Add(resI);
        }

        File.WriteAllLines("Assets/Editor/GrmAbbr.txt", sb.ToArray());
    }

}
