using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProcessCSVFile
{
    //去掉chapter文件title变量 换行
    [MenuItem("Assets/Tools/ProcessCSVChapter")]
    public static void ProcessCSV()
    {

        //支持多选
        string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
                                              //for (int i = 0; i < guids.Length; i++)
                                              //{
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
                                                                   //TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

        string[] textTxt = File.ReadAllLines(assetPath);

        List<string> res = new List<string>();
        int l = textTxt.Length;
        res.Add(textTxt[0]);
        for (int i = 1; i < l; i++)
        {
            while (i < l)
            {
                if (string.IsNullOrEmpty(textTxt[i]) || textTxt[i][textTxt[i].Length - 1] != '"')
                {
                    //textTxt[i] += textTxt[i+1];
                    textTxt[i + 1] = textTxt[i] + textTxt[i + 1];
                    ++i;
                }
                else
                    break;
            }
            res.Add(textTxt[i]);
        }


        //File.WriteAllLines(assetPath, res.ToArray());
        File.WriteAllLines("Assets/Editor/chapter - bak_P.csv", res.ToArray());






        //}
    }
    //去掉csv文件某些不需要的列(变量)
    [MenuItem("Assets/Tools/FilterCSV")]
    public static void FilterCSV()
    {

        //支持多选
        string[] guids = Selection.assetGUIDs;//获取当前选中的asset的GUID
                                              //for (int i = 0; i < guids.Length; i++)
                                              //{
        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);//通过GUID获取路径
                                                                   //TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

        string[] textTxt = File.ReadAllLines(assetPath);

        List<string> res = new List<string>();
        int l = textTxt.Length;
        for (int i = 0; i < l; i++)
        {
            string[] split = textTxt[i].Split(',');
            string resT = "";
            for (int j = 0; j < split.Length; j++)
            {
                if (j != 0 && j != 1)//tag_map
                                     //if (j != 5 && j != 7 && j != 8 && j != 10)//pali_text
                                     //if (j != 0 && j != 6 && j != 7 && j != 10 && j != 9 && j != 10 && j != 11)//sentence_translation
                {
                    //if (j != 0)
                    //if (j != 1)
                    if (j != 2)
                        resT += ("," + split[j]);
                    else
                        resT += split[j];
                }
            }
            res.Add(resT);
        }


        //File.WriteAllLines(assetPath, res.ToArray());
        File.WriteAllLines("Assets/Editor/tag_map - bak_P.csv", res.ToArray());






        //}
    }

}
