using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicController
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private DicController() { }
    private static DicController controller = null;
    //静态工厂方法 
    public static DicController Instance()
    {
        if (controller == null)
        {
            controller = new DicController();
        }
        return controller;
    }
    public DictManager dicManager = DictManager.Instance();




}
