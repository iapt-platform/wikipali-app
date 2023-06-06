using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkMangaer
{
    private NetworkMangaer() { }
    private static NetworkMangaer manager = null;
    //静态工厂方法 
    public static NetworkMangaer Instance()
    {
        if (manager == null)
        {
            manager = new NetworkMangaer();
        }
        return manager;
    }
    //判断当前是否联网

    // Use this for initialization
    public bool NetworkIsWiFi()
    {
        //当用户使用WiFi时
        if (Application.internetReachability == UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return true;
        }
        return false;
    }
    //判断当前是否联网
    public bool PingNetAddress()
    {
        if (Application.internetReachability == UnityEngine.NetworkReachability.NotReachable)
        {
       //     UITool.ShowToastMessage(GameManager.Instance(), "无网络连接", 35);

            return false;
        }
       // UITool.ShowToastMessage(GameManager.Instance(), "有网络连接", 35);

        return true;
        //try
        //{
        //    System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
        //    PingReply pr = ping.Send("www.baidu.com", 3000);
        //    if (pr.Status == IPStatus.Success)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //catch (Exception e)
        //{
        //    return false;
        //}
    }
    //判断是否是外网
    public bool PingOuterNet()
    {
        if (Application.internetReachability == UnityEngine.NetworkReachability.NotReachable)
        {
            return false;
        }
        try
        {
            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
            PingReply pr = ping.Send("www.google.com", 3000);
            if (pr.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
