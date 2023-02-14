using SevenZip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
//压缩与解压缩管理类
public class ZipManager : MonoBehaviour
{
    #region 压缩代码
    //    [SerializeField]
    //    Slider m_Slider;


    //    CodeProgress m_CodeProgress = null;//进度;

    //    Thread m_CompressThread = null; //压缩线程;
    //    Thread m_DecompressThread = null;   //解压缩线程;

    //    string ApplicationdataPath = string.Empty;

    //    private float m_Percent = 0f;

    //    // Use this for initialization
    //    void Start()
    //    {
    //        ApplicationdataPath = Application.dataPath;

    //        m_CodeProgress = new CodeProgress(SetProgressPercent);
    //    }

    //    void Update()
    //    {
    //        if (m_Percent > 0f)
    //        {
    //            m_Slider.value = m_Percent;
    //            if (m_Percent == 1f)
    //            {
    //                Debug.Log("m_Percent==1f");
    //                m_Percent = 0f;
    //                //?????????????
    //                //UnityEditor.AssetDatabase.Refresh();
    //            }
    //        }
    //    }


    //    void OnGUI()
    //    {
    //        if (GUI.Button(new Rect(100, 100, 100, 100), "Compress"))
    //        {
    //            m_Slider.value = 0f;
    //            m_CompressThread = new Thread(new ThreadStart(TestCompress));
    //            m_CompressThread.Start();
    //        }

    //        if (GUI.Button(new Rect(300, 100, 100, 100), "DeCompress"))
    //        {
    //            m_Slider.value = 0f;
    //            m_DecompressThread = new Thread(new ThreadStart(TestDeCompress));
    //            m_DecompressThread.Start();
    //        }
    //    }



    //    void TestCompress()
    //    {
    //        try
    //        {
    //            Compress(ApplicationdataPath + "/lib.UPK", ApplicationdataPath + "/lib.ZUPK");
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log(ex);
    //        }
    //    }

    //    void TestDeCompress()
    //    {
    //        try
    //        {
    //            DeCompress(ApplicationdataPath + "/lib.ZUPK", ApplicationdataPath + "/lib1.UPK");
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log(ex);
    //        }
    //    }


    //    void SetProgressPercent(Int64 fileSize, Int64 processSize)
    //    {
    //        m_Percent = (float)processSize / fileSize;
    //    }


    //    void Compress(string inpath, string outpath)
    //    {
    //        SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
    //        FileStream inputFS = new FileStream(inpath, FileMode.Open);
    //        FileStream outputFS = new FileStream(outpath, FileMode.Create);

    //        encoder.WriteCoderProperties(outputFS);

    //        outputFS.Write(System.BitConverter.GetBytes(inputFS.Length), 0, 8);

    //        encoder.Code(inputFS, outputFS, inputFS.Length, -1, m_CodeProgress);
    //        outputFS.Flush();
    //        outputFS.Close();
    //        inputFS.Close();
    //    }

    //    void DeCompress(string inpath, string outpath)
    //    {
    //        SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
    //        FileStream inputFS = new FileStream(inpath, FileMode.Open);
    //        FileStream outputFS = new FileStream(outpath, FileMode.Create);

    //        int propertiesSize = SevenZip.Compression.LZMA.Encoder.kPropSize;
    //        byte[] properties = new byte[propertiesSize];
    //        inputFS.Read(properties, 0, properties.Length);

    //        byte[] fileLengthBytes = new byte[8];
    //        inputFS.Read(fileLengthBytes, 0, 8);
    //        long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);

    //        decoder.SetDecoderProperties(properties);
    //        decoder.Code(inputFS, outputFS, inputFS.Length, fileLength, m_CodeProgress);
    //        outputFS.Flush();
    //        outputFS.Close();
    //        inputFS.Close();
    //    }


    //}

    //class CodeProgress : ICodeProgress
    //{
    //    public delegate void ProgressDelegate(Int64 fileSize, Int64 processSize);

    //    public ProgressDelegate m_ProgressDelegate = null;

    //    public CodeProgress(ProgressDelegate del)
    //    {
    //        m_ProgressDelegate = del;
    //    }

    //    public void SetProgress(Int64 inSize, Int64 outSize)
    //    {

    //    }

    //    public void SetProgressPercent(Int64 fileSize, Int64 processSize)
    //    {
    //        m_ProgressDelegate(fileSize, processSize);
    //    }
    #endregion
#if UNITY_EDITOR
    string filepath = Application.dataPath + "/StreamingAssets" + "/my.xml";
#elif UNITY_IPHONE
string filepath = Application.dataPath +"/Raw"+"/my.xml";
#elif UNITY_ANDROID
string filepath = “jar:file://” + Application.dataPath + “!/assets/”+"/my.xml;
#endif
    public void UnZip()
    {


    }


}

