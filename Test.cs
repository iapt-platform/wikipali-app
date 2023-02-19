using UnityEngine;
using System.Collections;
using YZL.Compress.GZip;
using YZL.Compress.LZMA;
using YZL.Compress.UPK;
public class Test : MonoBehaviour {


    /// <summary>
    /// 请使用异步压缩或者解压。
    /// </summary>
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2 - 210 , 200, 140, 80), "LMZA压缩文件"))
        {
            LZMAFile.CompressAsync(Application.dataPath + "/music.mp3", Application.dataPath + "/music.lzma", ShowProgress);
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 70, 200, 140, 80), "GZIP压缩文件"))
        {
            GZipFile.CompressAsync(Application.dataPath + "/music.mp3", Application.dataPath + "/music.gzip", ShowProgress);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 320, 140, 80), "LMZA解压文件"))
        {
            LZMAFile.DeCompressAsync(Application.dataPath + "/music.lzma", Application.dataPath + "/lzmamusic.mp3", ShowProgress);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 320, 140, 80), "GZIP解压文件"))
        {
            GZipFile.DeCompressAsync(Application.dataPath + "/music.gzip", Application.dataPath + "/gzipmusic.mp3", ShowProgress);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 210, 440, 140, 80), "UPK打包文件夹"))
        {
            UPKFolder.PackFolderAsync(Application.dataPath + "/picture", Application.dataPath + "/picture.upk", ShowProgress);
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 70, 440, 140, 80), "UPK解包文件夹"))
        {
            UPKFolder.UnPackFolderAsync(Application.dataPath + "/picture.upk", Application.dataPath + "/", ShowProgress);
        }

    }

    void ShowProgress(long all,long now)
    {
        double progress = (double)now /all;
        Debug.Log("当前进度为: " + progress);
    }
}
