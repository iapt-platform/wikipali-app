using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static ArticleController;

public class CommonTool
{
    //返回翻译标题
    //todo：不同语言 标题不同
    public static string GetBookTranslateName(Book book)
    {
        if (string.IsNullOrEmpty(book.translateName))
            return book.toc;
        else
            return book.translateName;
    }


    public static string CopyAndroidPathToPersistent(string datebasePath)
    {

        string path;


        path = Application.persistentDataPath + "/" + datebasePath;

        //如果查找该文件路径
        if (File.Exists(path))
        {
            Debug.LogError("找到了！" + path);
            //返回该数据库路径
            return path;
        }
        //Debug.LogError("1111111111wwww");
        //Debug.LogError("复制数据库路径" + "jar:file://" + Application.dataPath + "!/assets/" + datebasePath);

        // jar:file:是安卓手机路径的意思  
        // Application.dataPath + "!/assets/"   即  Application.dataPath/StreamingAssets  
        var request = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + datebasePath);
        request.SendWebRequest(); //读取数据
        while (!request.downloadHandler.isDone) { }
        // 因为安卓中streamingAssetsPath路径下的文件权限是只读，所以获取之后把他拷贝到沙盘路径中
        File.WriteAllBytes(path, request.downloadHandler.data);
        //Debug.LogError("复制完了" + path);
        //Debug.LogError("22222222222wwww");

        return path;
    }


    /// <summary>
    /// 对相机截图
    /// </summary>
    /// <param name="camera">Camera.要被截屏的相机</param>
    /// <param name="rect">Rect.截屏的区域</param>
    /// <returns>The screenshot2.</returns>
    public static Texture2D CaptureCamera(Camera camera, Rect rect, Vector3 cameraPos)
    {
        camera.transform.position = cameraPos;
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);//创建一个RenderTexture对象
        camera.targetTexture = rt;//临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
        //ps: camera2.targetTexture = rt;
        //ps: camera2.Render();
        //ps: -------------------------------------------------------------------

        RenderTexture.active = rt;//激活这个rt, 并从中中读取像素。
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);//注：这个时候，它是从RenderTexture.active中读取像素
        screenShot.Apply();

        //重置相关参数，以使用camera继续在屏幕上显示
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;

        RenderTexture.active = null; //JC: added to avoid errors
        GameObject.Destroy(rt);

        //byte[] bytes = screenShot.EncodeToPNG();//最后将这些纹理数据，成一个png图片文件
        //string filename = Application.dataPath + "/Screenshot.png";
        //System.IO.File.WriteAllBytes(filename, bytes);
        //Debug.Log(string.Format("截屏了一张照片: {0}", filename));

        return screenShot;
    }

    //todo ios代码
    /// <summary>
    /// 保存图片
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static string SaveImages(Texture2D texture)
    {
        string path = Application.streamingAssetsPath;
#if UNITY_EDITOR
            path = Application.dataPath;
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            path = "/sdcard/DCIM/Camera"; //设置图片保存到设备的目录.
#endif
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string savePath = path + "/" + texture.name + ".png";
        try
        {
            Application.HasUserAuthorization(UserAuthorization.Microphone);
            //byte[] data = DeCompress(texture).EncodeToPNG();
            byte[] data = texture.EncodeToPNG();
            File.WriteAllBytes(savePath, data);
            OnSaveImagesPlartform(savePath);
        }
        catch
        {
        }
        return savePath;
    }
    public static string SaveImages2PersistentDataPath(Texture2D texture)
    {
        string path = Application.persistentDataPath;
        //if (!Directory.Exists(path))
        //    Directory.CreateDirectory(path);
        string savePath = path + "/" + texture.name + ".png";
        try
        {
            Application.HasUserAuthorization(UserAuthorization.Microphone);
            //byte[] data = DeCompress(texture).EncodeToPNG();
            byte[] data = texture.EncodeToPNG();
            File.WriteAllBytes(savePath, data);
            OnSaveImagesPlartform(savePath);
        }
        catch
        {
        }
        return savePath;
    }
    /// <summary>
    /// 刷新相册（不需要单独创建原生aar或jar）
    /// </summary>
    /// <param name="path"></param>
    static void OnSaveImagesPlartform(string filePath)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            string[] paths = new string[1];
            paths[0] = filePath; 
            using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
                {
                    Conn.CallStatic("scanFile", playerActivity, paths, null, null);
                }
            }
#endif
    }
    /// <summary>
    /// 压缩图片
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    static Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
    /// <summary>
    /// 从外部指定文件中加载图片
    /// </summary>
    /// <returns></returns>
    public static Texture2D LoadTextureByIO(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);//游标的操作，可有可无
        byte[] bytes = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
        try
        {
            fs.Read(bytes, 0, bytes.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        fs.Close();//切记关闭

        int width = 2048;//图片的宽（这里两个参数可以提到方法参数中）
        int height = 2048;//图片的高（这里说个题外话，pico相关的开发，这里不能大于4k×4k不然会显示异常，当时开发pico的时候应为这个问题找了大半天原因，因为美术给的图是6000*3600，导致出现切几张图后就黑屏了。。。
        Texture2D texture = new Texture2D(width, height);
        if (texture.LoadImage(bytes))
        {
            //print("图片加载完毕 ");
            return texture;//将生成的texture2d返回，到这里就得到了外部的图片，可以使用了

        }
        else
        {
            //print("图片尚未加载");
            return null;
        }
    }
}
