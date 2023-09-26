using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using ZXing.Common;
//保存二进制文件的工具
public class SaveBytes : MonoBehaviour
{
    //key做加密，所以转换后的bytes所有奇数位偶数位互换，读取后也要互换
    [MenuItem("Assets/Tools/SaveBytes")]
    public static void SaveBytesFile()
    {
        string str = "";
        str = CommonTool.SwapString(str);
        //todo 隐藏密码
        CommonTool.SerializeObjectToFile(@"Assets/Editor/font.font", str, "wikipaliapp12345");
    }

    [MenuItem("Assets/Tools/LoadBytes")]
    public static void LoadBytesFile()
    {

        string str = (string)CommonTool.DeserializeObjectFromFile("Assets/Editor/font.font", "wikipaliapp12345");
        str = CommonTool.SwapString(str);
        Debug.LogError(str);
    }
}
