using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

/// <summary>
/// 创建二维码
/// </summary>
public class CreatQR
{

    //存放二维码的纹理图片
    public static Texture2D encoded = new Texture2D(256, 256);

    [Header("需要生产二维码的字符")]
    public static string QrCodeStr = "https://www-hk.wikipali.org";
    [Header("在屏幕上显示二维码 ")]
    public RawImage rawImg;

    //void Start()
    //{
    //    /*初始化纹理图片
    //     * 注意：宽高度大小必须是256，
    //     * 否则出现索引超出数组边界错误
    //     */
    //    encoded = new Texture2D(256, 256);
    //    CreatQr(); //创建生成二维码
    //}

    #region 生成二维码
    public static bool LoadQR()
    {
        string path = Application.persistentDataPath + "/wpa_QR.png";
        if (!File.Exists(path))
            return false;
        encoded = CommonTool.LoadTextureByIO(path);
        return true;
    }
    /// <summary>  
    /// 创建二维码
    /// </summary>  
    public static void CreatQr()
    {
        if (QrCodeStr != string.Empty)
        {
            //二维码写入图片    
            var color32 = Encode(QrCodeStr, encoded.width, encoded.height);
            encoded.SetPixels32(color32); //更改纹理的像素颜色
            encoded.Apply();
            encoded.name = "wpa_QR";
            CommonTool.SaveImages2PersistentDataPath(encoded);
            //生成的二维码图片附给RawImage    
            //rawImg.texture = encoded;
        }
        else
            Debug.Log("没有生成信息");
    }

    /// <summary>
    /// 生成二维码 
    /// </summary>
    /// <param name="textForEncoding">需要生产二维码的字符串</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns></returns>       
    private static Color32[] Encode(string formatStr, int width, int height)
    {

        //绘制二维码前进行一些设置
        QrCodeEncodingOptions options = new QrCodeEncodingOptions();

        //设置字符串转换格式，确保字符串信息保持正确
        options.CharacterSet = "UTF-8";

        //设置绘制区域的宽度和高度的像素值
        options.Width = width;
        options.Height = height;

        //设置二维码边缘留白宽度（值越大留白宽度大，二维码就减小）
        options.Margin = 1;

        /*实例化字符串绘制二维码工具
         * BarcodeFormat:条形码格式
         * Options： 编码格式（支持的编码格式）
         */
        var barcodeWriter = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = options };
        //进行二维码绘制并进行返回图片的颜色数组信息
        return barcodeWriter.Write(formatStr);

    }
    #endregion
}