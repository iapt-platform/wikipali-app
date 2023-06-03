﻿/// <summary>
///
//      ALIyerEdon - Winter 2017 - Orginally writed for OBB Downloader
//
/// </summary>
using UnityEngine;
using System.Collections;
using System.Net;
using System.ComponentModel;
using UnityEngine.UI;
using System.IO;
using System;

public class DownloadManager 
{
    private DownloadManager() { }
    private static DownloadManager manager = null;
    //静态工厂方法 
    public static DownloadManager Instance()
    {
        if (manager == null)
        {
            manager = new DownloadManager();
        }
        return manager;
    }
    #region 公开方法
    //同时只能下载一个文件
    Func<object, object> downLoadFinFunc;
    public void DownLoad(string _savePath,string _downLoadUrl, Func<object,object> _downLoadFinFunc,string fileName = "")
    {
        downLoadFinFunc = _downLoadFinFunc;
        Init(_savePath, _downLoadUrl, fileName);
        DownloadFile();
    }
    #endregion
    #region 下载核心代码

    //[Header("Informations")]
    public string savePath = "C:/Example";
    public string downloadUrl = "http://89.163.206.23/CarParkingKit1.5.1.zip";

    //[Header("Options")]
    public bool persistentDataPath = false;
    public bool showBytes = true;
    public bool onStart = false;

    //[Header("Display")]
    //public Slider progressBar;

    //public Text progressText;
    //public Text bytesText;

    //[Header("Finish")]
    // Activate this button when download has finished
    public GameObject finishedButton;
    // De Activate this button when download has finished
    public GameObject downloadButton;

    //public bool UseOrginalName = true;
    // File name to save file in location + extension (.exe ,.zip and ...)
    public string newFileName = "example.zip";


    // internal usage
    string uri;
    float progress;
    string bytes;
    bool downloading;
    bool finished;
    public void Init(string _savePath, string _downloadUrl,string _fileName)
    {
        // Set progress bar (UI Slider) max value to 100 (%) 
        //if (progressBar.maxValue != 100f)
        //    progressBar.maxValue = 100f;

        //// Starting value is 0
        //progressBar.value = 0;
        downloadUrl = _downloadUrl;
        uri = downloadUrl;

        // Use orginal downloaded file name or not
        //if (UseOrginalName)
        if (string.IsNullOrEmpty(_fileName))
            newFileName = Path.GetFileName(uri);
        else
            newFileName = _fileName;

      // Check directory exists
        savePath = _savePath;
        DirectoryInfo df = new DirectoryInfo(savePath);
        if (!df.Exists)
            Directory.CreateDirectory(savePath);


        if (onStart)
            DownloadFile();


    }

    //void Start()
    //{
    //    // Set progress bar (UI Slider) max value to 100 (%) 
    //    if (progressBar.maxValue != 100f)
    //        progressBar.maxValue = 100f;

    //    // Starting value is 0
    //    progressBar.value = 0;

    //    uri = downloadUrl;

    //    // Use orginal downloaded file name or not
    //    if (UseOrginalName)
    //        newFileName = Path.GetFileName(uri);

    //    // Check directory exists
    //    DirectoryInfo df = new DirectoryInfo(savePath);
    //    if (!df.Exists)
    //        Directory.CreateDirectory(savePath);


    //    if (onStart)
    //        DownloadFile();

    //}
    WebClient client;
    string realSavePath;
    // Main download function (public for ui button)     
     void DownloadFile()
    {
        downloadButton.SetActive(false);

        cancelled = false;

        client = new WebClient();

        if (!persistentDataPath)
        {
            realSavePath = savePath + "/" + newFileName;
            client.DownloadFileAsync(new System.Uri(downloadUrl), realSavePath);
        }
        else
        {
            realSavePath = Application.persistentDataPath + "/" + newFileName;
            client.DownloadFileAsync(new System.Uri(uri), realSavePath);
        }

        downloading = true;
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
        client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCompleted);


    }

    // Manage download state on unity main thread
    //void Update()
    //{
    //    if (downloading)
    //    {
    //        progressBar.value = progress;
    //        progressText.text = progress.ToString() + "% ";

    //        if (showBytes)
    //            bytesText.text = "Recieved : " + bytes + " kb";
    //    }


    //    if (finished)
    //    {
    //        if (cancelled)
    //        {
    //            bytesText.text = "Canceled";
    //            progressText.text = "0 %";
    //            finished = false;
    //        }
    //        else
    //        {
    //            if (!finishedButton.activeSelf)
    //            {
    //                finishedButton.SetActive(true);
    //                downloadButton.SetActive(false);
    //            }
    //            bytesText.text = "Recieved : " + "Completed";
    //            progressText.text = "100 %";
    //        }
    //    }

    //}
    void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (cancelled)
        {
            Debug.Log("Canceled");
            downloading = false;
            finished = true;

        }
        else
        {
            if (e.Error == null)
            {
                Debug.Log("Completed");
                downloading = false;
                finished = true;
                downLoadFinFunc(realSavePath);
            }
            else
                Debug.Log(e.Error.ToString());
        }
    }

    void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        progress = (e.BytesReceived * 100 / e.TotalBytesToReceive);
        bytes = e.BytesReceived / 1000 + " / " + e.TotalBytesToReceive / 1000;
    }

    // Use this for game start button when download has finished
    public void LoadLevel(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    bool cancelled;

    public void CancelDownload()
    {
        cancelled = true;
        if (client != null)
        {
            client.CancelAsync();


            /*if(!persistentDataPath)
				File.Delete ( savePath+ "/" + newFileName);
			else
				File.Delete ( Application.persistentDataPath+ "/" + newFileName);
			*/
        }

        downloadButton.SetActive(true);

    }

    // Cancel download when script has disabled
    void OnDisable()
    {
        cancelled = true;
        if (client != null)
            client.CancelAsync();
    }

    #endregion
}
