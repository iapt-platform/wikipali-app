using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkMangaer
{
    private NetworkMangaer() { }
    private static NetworkMangaer manager = null;
    //��̬�������� 
    public static NetworkMangaer Instance()
    {
        if (manager == null)
        {
            manager = new NetworkMangaer();
        }
        return manager;
    }
    //�жϵ�ǰ�Ƿ�����

    // Use this for initialization
    public bool NetworkIsWiFi()
    {
        //���û�ʹ��WiFiʱ
        if (Application.internetReachability == UnityEngine.NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return true;
        }
        return false;
    }
    //�жϵ�ǰ�Ƿ�����
    public bool PingNetAddress()
    {
        if (Application.internetReachability == UnityEngine.NetworkReachability.NotReachable)
        {
       //     UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);

            return false;
        }
       // UITool.ShowToastMessage(GameManager.Instance(), "����������", 35);

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
    //�ж��Ƿ�������
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
