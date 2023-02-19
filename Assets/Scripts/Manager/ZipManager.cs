
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
//压缩与解压缩管理类
public class ZipManager
{
    #region 压缩代码
    //[SerializeField]
    //Slider m_Slider;


    //CodeProgress m_CodeProgress = null;//进度;

    //Thread m_CompressThread = null; //压缩线程;
    //Thread m_DecompressThread = null;   //解压缩线程;

    //string ApplicationdataPath = string.Empty;

    //private float m_Percent = 0f;

    //// Use this for initialization
    //void Start()
    //{
    //    ApplicationdataPath = Application.dataPath;

    //    m_CodeProgress = new CodeProgress(SetProgressPercent);
    //}

    //void Update()
    //{
    //    if (m_Percent > 0f)
    //    {
    //        m_Slider.value = m_Percent;
    //        if (m_Percent == 1f)
    //        {
    //            Debug.Log("m_Percent==1f");
    //            m_Percent = 0f;
    //            //?????????????
    //            //UnityEditor.AssetDatabase.Refresh();
    //        }
    //    }
    //}


    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(100, 100, 100, 100), "Compress"))
    //    {
    //        m_Slider.value = 0f;
    //        m_CompressThread = new Thread(new ThreadStart(TestCompress));
    //        m_CompressThread.Start();
    //    }

    //    if (GUI.Button(new Rect(300, 100, 100, 100), "DeCompress"))
    //    {
    //        m_Slider.value = 0f;
    //        m_DecompressThread = new Thread(new ThreadStart(TestDeCompress));
    //        m_DecompressThread.Start();
    //    }
    //}



    //void TestCompress()
    //{
    //    try
    //    {
    //        Compress(ApplicationdataPath + "/lib.UPK", ApplicationdataPath + "/lib.ZUPK");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log(ex);
    //    }
    //}

    //void TestDeCompress()
    //{
    //    try
    //    {
    //        DeCompress(ApplicationdataPath + "/lib.ZUPK", ApplicationdataPath + "/lib1.UPK");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log(ex);
    //    }
    //}


    //void SetProgressPercent(Int64 fileSize, Int64 processSize)
    //{
    //    m_Percent = (float)processSize / fileSize;
    //}


    //void Compress(string inpath, string outpath)
    //{
    //    SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
    //    FileStream inputFS = new FileStream(inpath, FileMode.Open);
    //    FileStream outputFS = new FileStream(outpath, FileMode.Create);

    //    encoder.WriteCoderProperties(outputFS);

    //    outputFS.Write(System.BitConverter.GetBytes(inputFS.Length), 0, 8);

    //    encoder.Code(inputFS, outputFS, inputFS.Length, -1, m_CodeProgress);
    //    outputFS.Flush();
    //    outputFS.Close();
    //    inputFS.Close();
    //}

    //void DeCompress(string inpath, string outpath)
    //{
    //    SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
    //    FileStream inputFS = new FileStream(inpath, FileMode.Open);
    //    FileStream outputFS = new FileStream(outpath, FileMode.Create);

    //    int propertiesSize = SevenZip.Compression.LZMA.Encoder.kPropSize;
    //    byte[] properties = new byte[propertiesSize];
    //    inputFS.Read(properties, 0, properties.Length);

    //    byte[] fileLengthBytes = new byte[8];
    //    inputFS.Read(fileLengthBytes, 0, 8);
    //    long fileLength = System.BitConverter.ToInt64(fileLengthBytes, 0);

    //    decoder.SetDecoderProperties(properties);
    //    decoder.Code(inputFS, outputFS, inputFS.Length, fileLength, m_CodeProgress);
    //    outputFS.Flush();
    //    outputFS.Close();
    //    inputFS.Close();
    //}



    #endregion
#if UNITY_EDITOR
    string filepath = Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
    string filepath = Application.dataPath +"/Raw/";
#elif UNITY_ANDROID
    string filepath = "jar:file://" + Application.dataPath + "!/assets/";
#endif

    private ZipManager() { }
    private static ZipManager manager = null;
    //静态工厂方法 
    public static ZipManager Instance()
    {
        if (manager == null)
        {
            manager = new ZipManager();
        }
        return manager;
    }
    public void UnZipDB()
    {
        GameManager.Instance().StartUnZipDB();
        //安卓中需要把StreamingAsset路径下数据库压缩包拷贝到PersistentAsset下面，因为前者文件夹只读不能解压，后者文件夹读写都可以
#if UNITY_ANDROID && !UNITY_EDITOR
        filepath = CommonTool.CopyAndroidPathToPersistent("DB.7z");
        filepath = filepath.Replace("DB.7z","");
        Debug.LogError("复制到" + filepath);
#endif
        sizeOfEntry = lzma.get7zSize(filepath + "DB.7z");
        Debug.LogError("大小" + sizeOfEntry);
        DecompressDBLZMA();
    }
    public int[] fileProgress = new int[1];
    private Thread th = null;
    public ulong sizeOfEntry;
    private void DecompressDBLZMA()
    {
        Debug.LogError("开始解压");
        fileProgress[0] = 0;
        th = new Thread(Decompress); th.Start(); // faster then coroutine
    }
    void Decompress()
    {
        Debug.LogError("线程开始");

        int lzres = lzma.doDecompress7zip(filepath + "DB.7z", filepath, fileProgress, true, true);
        if (lzres == 1)
            DeCompressFin();
    }
    void DeCompressFin()
    {
        fileProgress[0] = 100;

        //GameManager.Instance().EndUnZipDB();
    }

}
