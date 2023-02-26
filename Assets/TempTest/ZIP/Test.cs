using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    public Object asset;
    public Object asset2;
    public Object asset3;
    public Slider slider;

    //string path = "jar:file://" + Application.dataPath + "!/assets/DB/Dict";
    //string path = "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Dict";
    //string path = "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB\\Sentence";
    string pathFolder = "E:\\UnityProject\\WikiPali\\Assets\\StreamingAssets\\DB";
    private string appDBPath(string datebasePath, string datebaseName)
    {

        string path;


        path = Application.persistentDataPath + "/" + datebaseName;

        //如果查找该文件路径
        if (File.Exists(path))
        {
            Debug.LogError("找到了！" + path);
            //返回该数据库路径
            return path;
        }
        Debug.LogError("1111111111wwww");
        Debug.LogError("复制数据库路径" + "jar:file://" + Application.dataPath + "!/assets/" + datebasePath);

        // jar:file:是安卓手机路径的意思  
        // Application.dataPath + "!/assets/"   即  Application.dataPath/StreamingAssets  
        var request = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + datebasePath);
        request.SendWebRequest(); //读取数据
        while (!request.downloadHandler.isDone) { }
        // 因为安卓中streamingAssetsPath路径下的文件权限是只读，所以获取之后把他拷贝到沙盘路径中
        File.WriteAllBytes(path, request.downloadHandler.data);
        Debug.LogError("复制完了" + path);
        Debug.LogError("22222222222wwww");

        return path;
    }
    private void Start()
    {
        Debug.LogError("Start!!!!!!!!!!!!!!");

    }
    //A 1 item integer array to get the current extracted file of the 7z archive. Compare this to the total number of the files to get the progress %.
    private int[] fileProgress = new int[1];
    void DoDecompression(string filePath,string dirPath)
    {

        // Decompress the 7z file
        int lzres;
        float t = Time.realtimeSinceStartup;

        // the referenced progress int will indicate the current index of file beeing decompressed. Use in a separate thread to show it realtime.

        // to get realtime byte level decompression progress (from a thread), there are 2 ways:
        //
        // 1. use lzma.get7zSize function to get the total uncompressed size of the files and compare against the bytes written in realtime, calling the lzma.getBytesWritten function.
        //
        // 2. use the lzma.getFileSize (or buffer length for FileBuffers) to get the file size and compare against the bytes read in realtime, calling the lzma.getBytesRead function.
        fileProgress[0] = 0;

        lzres = lzma.doDecompress7zip(filePath, dirPath, fileProgress, true, true);

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 200, 140, 80), "LMZA解压文件 p1"))
        {
            //LZMAFile.CompressAsync(path + ".db", path + ".lzma", ShowProgress);
            //string p3 = appDBPath("DB/Dict.lzma", "Dict.lzma");
            string datebasePath = "DB.7z";
            string p = "jar:file://" + Application.dataPath + "!/assets/" + datebasePath;
            Debug.LogError(p);
            string p2 = Application.persistentDataPath + "/";
            Debug.LogError(p2);
            DoDecompression(p, p2);
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 70, 200, 140, 80), "GZIP解压文件 p1"))
        {
            //LZMAFile.CompressAsync(path + ".db", path + ".lzma", ShowProgress);
            //string p3 = appDBPath("DB/Dict.lzma", "Dict.lzma");
            string p3 = appDBPath("DB.7z", "DB.7z");
            Debug.LogError(p3);
            string p2 = p3.Replace(".7z", "");
            Debug.LogError(p2);
            DoDecompression(p3, p2);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 320, 140, 80), "LMZA解压文件 p2"))
        {
            string p3 = appDBPath("DB/Dict.7z", "Dict.7z");
            string p = p3;// + ".lzma";
            Debug.LogError(p);
            string p2 = p3.Replace("Dict.7z", "");
            Debug.LogError(p2);
            DoDecompression(p, p2);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 320, 140, 80), "GZIP解压文件 p2"))
        {
            //LZMAFile.CompressAsync(path + ".db", path + ".lzma", ShowProgress);
            //string p3 = appDBPath("DB/Dict.lzma", "Dict.lzma");
            string p = "file://" + Application.dataPath + "!/assets/DB/Dict.7z";
            Debug.LogError(p);
            string p2 = "file://" + Application.dataPath + "!/assets/DB/";
            Debug.LogError(p2);
            DoDecompression(p, p2);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 440, 140, 80), "LMZA解压文件 p3"))
        {
            string p = Application.streamingAssetsPath + "/DB.7z";
            Debug.LogError(p);
            string p2 = Application.streamingAssetsPath + "/";
            Debug.LogError(p2);
            Debug.LogError("开始解压");
#if DEBUG_PERFORMANCE || UNITY_EDITOR
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            DoDecompression(p, p2);
            Debug.LogError("解压完成");
#if DEBUG_PERFORMANCE || UNITY_EDITOR
            sw.Stop();
            Debug.LogError("【性能】解压耗时：" + sw.ElapsedMilliseconds);
#endif
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 440, 140, 80), "GZIP解压文件 p3"))
        {
            string p = Application.streamingAssetsPath + "/Dict.7z";
            Debug.LogError(p);
            string p2 = Application.streamingAssetsPath + "/";
            Debug.LogError(p2);
            DoDecompression(p, p2);
        }
        //if (GUI.Button(new Rect(Screen.width / 2 - 210, 440, 140, 80), "UPK打包文件夹"))
        //{
        //    UPKFolder.PackFolderAsync(pathFolder, pathFolder + ".upk", ShowProgress);
        //}
        //if (GUI.Button(new Rect(Screen.width / 2 + 70, 440, 140, 80), "UPK解包文件夹"))
        //{
        //    UPKFolder.UnPackFolderAsync(pathFolder + ".upk", pathFolder + "\\", ShowProgress);
        //}

    }
    void ShowProgress(long all, long now)
    {
        float progress = (float)now / all;
        //slider.value = progress;
        Debug.Log("当前进度为: " + progress);
    }
}
