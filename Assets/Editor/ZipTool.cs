
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class ZipTool
{
    [MenuItem("Assets/Tools/TestZip")]
    public static void PrintDateString()
    {
        //ZipManager.Instance().UnZipDB();
        Debug.LogError(lzma.LzmaUtilDecode(Application.dataPath + "/StreamingAssets/Dict.lzma",
              Application.dataPath + "/StreamingAssets/Dict.db"));
    }


}
