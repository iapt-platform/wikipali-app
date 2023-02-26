using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle
{

    /// <summary>
    /// 编辑器扩展，该脚本需要放到Editor文件夹下
    /// 需要注意的：放在Editor文件夹下的脚本在打包时不会被发布出去（也就是不会被打包出去）
    /// </summary>
    [MenuItem("Bulid/Bulid AssetBundles")]
    //unity的编辑器扩展，使用的时候需要引入命名空间using UnityEditor
    //当点击Bulid AssetBundles的时候，会执行下面的方法
    static void BulidAllAssetBundles()
    {
        //打包AssetBundles的方法
        //unity5里面的BuildPipeline方法
        //测试本地加载 方法 StreamingAssets的路径下
        //宏定义 选择发布平台：这样写可以在发布不同平台的时候不需要修改代码
#if UNITY_ANDROID//安卓平台
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,
        //LZMA压缩方式
        BuildAssetBundleOptions.None,
        BuildTarget.Android);
#elif UNITY_IPHONE//iOS平台
    BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,
    BuildAssetBundleOptions.UncompressedAssetBundle,
    BuildTarget.IOS);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        BuildPipeline.BuildAssetBundles(
            Application.streamingAssetsPath,//打包后文件的存放路径
            BuildAssetBundleOptions.UncompressedAssetBundle,//是否压缩
            BuildTarget.StandaloneWindows);//标准window平台，AssetBundles资源在不同平台下不共享
#endif
    }


}
