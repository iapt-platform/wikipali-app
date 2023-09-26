using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

/// <summary>
/// ������ά��
/// </summary>
public class CreatQR
{

    //��Ŷ�ά�������ͼƬ
    public static Texture2D encoded = new Texture2D(256, 256);

    [Header("��Ҫ������ά����ַ�")]
    public static string QrCodeStr = "https://www-hk.wikipali.org";
    [Header("����Ļ����ʾ��ά�� ")]
    public RawImage rawImg;

    //void Start()
    //{
    //    /*��ʼ������ͼƬ
    //     * ע�⣺��߶ȴ�С������256��
    //     * �������������������߽����
    //     */
    //    encoded = new Texture2D(256, 256);
    //    CreatQr(); //�������ɶ�ά��
    //}

    #region ���ɶ�ά��
    public static bool LoadQR()
    {
        string path = Application.persistentDataPath + "/wpa_QR.png";
        if (!File.Exists(path))
            return false;
        encoded = CommonTool.LoadTextureByIO(path);
        return true;
    }
    /// <summary>  
    /// ������ά��
    /// </summary>  
    public static void CreatQr()
    {
        if (QrCodeStr != string.Empty)
        {
            //��ά��д��ͼƬ    
            var color32 = Encode(QrCodeStr, encoded.width, encoded.height);
            encoded.SetPixels32(color32); //���������������ɫ
            encoded.Apply();
            encoded.name = "wpa_QR";
            CommonTool.SaveImages2PersistentDataPath(encoded);
            //���ɵĶ�ά��ͼƬ����RawImage    
            //rawImg.texture = encoded;
        }
        else
            Debug.Log("û��������Ϣ");
    }

    /// <summary>
    /// ���ɶ�ά�� 
    /// </summary>
    /// <param name="textForEncoding">��Ҫ������ά����ַ���</param>
    /// <param name="width">��</param>
    /// <param name="height">��</param>
    /// <returns></returns>       
    private static Color32[] Encode(string formatStr, int width, int height)
    {

        //���ƶ�ά��ǰ����һЩ����
        QrCodeEncodingOptions options = new QrCodeEncodingOptions();

        //�����ַ���ת����ʽ��ȷ���ַ�����Ϣ������ȷ
        options.CharacterSet = "UTF-8";

        //���û�������Ŀ�Ⱥ͸߶ȵ�����ֵ
        options.Width = width;
        options.Height = height;

        //���ö�ά���Ե���׿�ȣ�ֵԽ�����׿�ȴ󣬶�ά��ͼ�С��
        options.Margin = 1;

        /*ʵ�����ַ������ƶ�ά�빤��
         * BarcodeFormat:�������ʽ
         * Options�� �����ʽ��֧�ֵı����ʽ��
         */
        var barcodeWriter = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = options };
        //���ж�ά����Ʋ����з���ͼƬ����ɫ������Ϣ
        return barcodeWriter.Write(formatStr);

    }
    #endregion
}