using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 圣典控制类 单例
/// </summary>
public class ArticleController
{
    //懒汉式单例类.在第一次调用的时候实例化自己 
    private ArticleController() { }
    private static ArticleController controller = null;
    //静态工厂方法 
    public static ArticleController Instance()
    {
        if (controller == null)
        {
            controller = new ArticleController();
        }
        return controller;
    }

    //文章标题树形结构
    public class ArticleTreeNode
    {
        public List<ArticleTreeNode> children;
        public int lv;
    }



}
