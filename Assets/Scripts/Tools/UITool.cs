using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool
{
    //创建一个提示弹窗
    //确定，取消,,
    //参数是回调函数
    public delegate void PopOutFunc(bool boolean);
    public static void PopOutView(PopOutFunc Func)
    {
        Func(true);
    
    }




}
