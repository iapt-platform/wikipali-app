using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using ZXing.Common;
//����������ļ��Ĺ���
public class SaveBytes : MonoBehaviour
{
    //key�����ܣ�����ת�����bytes��������λż��λ��������ȡ��ҲҪ����
    [MenuItem("Assets/Tools/SaveBytes")]
    public static void SaveBytesFile()
    {
        string str = "";
        str = CommonTool.SwapString(str);
        //todo ��������
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
