//using cn.sharesdk.unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ShareView : MonoBehaviour
{

    [Header("控件部分")]
    //分享主界面
    public GameObject startShareLayer;
    //点击分享，分享到哪
    public GameObject shareBtnsLayer;
    //选择分享模板
    public GameObject selectModeLayer;
    //截图的部分
    public GameObject captureImgLayer;
    //选择词典的部分
    public ShareSelectPopView selectDicPopLayer;
    //整体返回
    public Button returnBtn;
    public Button shareBtn;
    public ShareTool shareTool;
    //组成图片的部分
    [Header("组成图片的部分")]
    public Text wordNameText;
    public Text dicNameText;
    public Text dicText;
    public Image backImg;
    public Material[] backImgMatArr;
    public Button[] modeBtnArr;
    public RectTransform shareCompRT;
    public RectTransform backImgBackRT;
    public RectTransform backImgRT;
    public GameObject tempParents;
    public Camera mainCamera;
    //填充的图片的基本高度
    float fillImgBaseHeight;
    float test;
    float test2;

    //分享的部分
    [Header("分享的部分")]
    public Button shareWeiXin;
    public Button shareWeiXinPYQ;
    public Button shareQQ;
    public Button shareQQKJ;
    public Button shareSina;
    public Button saveImg;
    public Button backBtn;

    public void Init()
    {
        startShareLayer.SetActive(false);
        shareBtnsLayer.SetActive(false);
        selectModeLayer.SetActive(false);
        //captureImgLayer.SetActive(false);
        selectDicPopLayer.gameObject.SetActive(true);
        selectDicPopLayer.Init();
    }
    // Start is called before the first frame update
    void Awake()
    {
        fillImgBaseHeight = backImgBackRT.localPosition.y - backImgBackRT.sizeDelta.y * 0.5f;
        test = backImgBackRT.localPosition.y;
        test2 = backImgRT.localPosition.y;

        shareWeiXin.onClick.AddListener(OnShareWeChat);
        shareWeiXinPYQ.onClick.AddListener(OnShareWeChatMoments);
        shareQQ.onClick.AddListener(OnShareQQ);
        shareQQKJ.onClick.AddListener(OnShareQZone);
        shareSina.onClick.AddListener(OnShareSina);

        saveImg.onClick.AddListener(OnSaveImgBtn);
        backBtn.onClick.AddListener(OnBackBtn);
        returnBtn.onClick.AddListener(OnReturnBtn);
        shareBtn.onClick.AddListener(OnShareBtn);
        int l = modeBtnArr.Length;
        for (int i = 0; i < l; i++)
        {
            //直接传入i会导致结果变为最大(循环最后的结果)
            int test = i;
            modeBtnArr[i].onClick.AddListener(() => { /*Debug.LogError(test); */OnModeBtnClick(test); });
        }
    }
    public void OnReturnBtn()
    {
        this.gameObject.SetActive(false);
    }
    public void OnBackBtn()
    {
        shareBtnsLayer.SetActive(false);
    }
    public void OnShareBtn()
    {
        shareBtnsLayer.SetActive(true);
    }
    public void OnSaveImgBtn()
    {
        ImgShot();
    }
    public void OnModeBtnClick(int id)
    {
        //Debug.LogError(id);
        backImg.material = backImgMatArr[id];
        SetBackImgMatParam(backImgRT.sizeDelta.x, backImgRT.sizeDelta.y);

    }
    //public Vector2 testRect = new Vector2();
    //public Vector2 testRead = new Vector2();
    //截图
    string imgPath;
    void ImgShot()
    {
        GameObject tempShotGo = GameObject.Instantiate(captureImgLayer, tempParents.transform);
        RectTransform rt = tempShotGo.GetComponent<RectTransform>();
        Vector2 size = rt.sizeDelta;
        rt.anchorMin = new Vector2(rt.anchorMin.x, 1);
        rt.anchorMax = new Vector2(rt.anchorMax.x, 1);
        rt.position = Vector3.zero;// new Vector3(size.x * 0.5f, 0, 0);
        //rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
        rt.localPosition = new Vector3(rt.localPosition.x, 0, 0);
        float yDivisionx = size.y / size.x;
        float screenYDx = (float)Screen.height / (float)Screen.width;
        Texture2D shot;
        if (screenYDx < yDivisionx)//长度超过屏幕长度
        {
            float ratio = ((float)Screen.width / size.x) * 1.01f;
            //?????????????????????
            float ratio2 = 1170.0f / (float)Screen.width;
            rt.localScale *= ratio * ratio2;
            rt.localPosition += Vector3.down * size.y * 0.5f * ratio * ratio2;// + Vector3.up * 100;// *0.75f;// * 0.5f;
            //??????????????????????????????
            shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, size.x - 20, size.y - 20 * ratio2 - 10 * yDivisionx), mainCamera.transform.position);

        }
        else//长度不超过屏幕长度
        {
            float ratio = ((float)Screen.width / size.x) * 1.01f;
            //?????????????????????
            float ratio2 = 1170.0f / (float)Screen.width;
            rt.localScale *= ratio * ratio2;
            rt.localPosition += Vector3.down * size.y * 0.5f * ratio * ratio2;// + Vector3.up * 100;// *0.75f;// * 0.5f;
            shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, size.x * 2, size.y * 2), mainCamera.transform.position);

        }
        //Debug.LogError(size.y * 0.5f);
        //Texture2D shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, backImgRT.sizeDelta.x, backImgRT.sizeDelta.y), mainCamera.transform.position);

        GameObject.Destroy(tempShotGo);
        DateTime NowTime = DateTime.Now.ToLocalTime();
        shot.name = "wpa_" + NowTime.Year + NowTime.Month + NowTime.Day + NowTime.Hour + NowTime.Minute + NowTime.Second;
        imgPath = CommonTool.SaveImages(shot);
        UITool.ShowToastMessage(this, "图片已保存\r\n" + imgPath, 35);
    }
    /* unity 2018 verson
    void ImgShot()
    {
        GameObject tempShotGo = GameObject.Instantiate(captureImgLayer, tempParents.transform);
        RectTransform rt = tempShotGo.GetComponent<RectTransform>();
        Vector2 size = rt.sizeDelta;
        rt.anchorMin = new Vector2(rt.anchorMin.x, 1);
        rt.anchorMax = new Vector2(rt.anchorMax.x, 1);
        rt.position = Vector3.zero;// new Vector3(size.x * 0.5f, 0, 0);
        //rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0);
        rt.localPosition = new Vector3(rt.localPosition.x, 0, 0);
        float yDivisionx = size.y / size.x;
        float screenYDx = (float)Screen.height / (float)Screen.width;
        Texture2D shot;
        if (screenYDx < yDivisionx)//长度超过屏幕长度
        {
            float ratio = ((float)Screen.height / size.y) * 1.001f;
            //?????????????????????
            float ratio2 = 1170.0f / (float)Screen.width;
            rt.localScale *= ratio * ratio2;
            rt.localPosition += Vector3.down * size.y * 0.5f * ratio * ratio2;// + Vector3.up * 100;// *0.75f;// * 0.5f;
            shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, size.x - 20, size.y - 10), mainCamera.transform.position);

        }
        else//长度不超过屏幕长度
        {
            float ratio = ((float)Screen.height / size.y) * 1.01f;
            //?????????????????????
            float ratio2 = 1170.0f / (float)Screen.width;
            rt.localScale *= ratio * ratio2;
            rt.localPosition += Vector3.down * size.y * 0.5f * ratio * ratio2;// + Vector3.up * 100;// *0.75f;// * 0.5f;
            shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, size.x * 2, size.y * 2), mainCamera.transform.position);

        }
        //Debug.LogError(size.y * 0.5f);
        //Texture2D shot = CommonTool.CaptureCamera(mainCamera, new Rect(0, 0, backImgRT.sizeDelta.x, backImgRT.sizeDelta.y), mainCamera.transform.position);

        //GameObject.Destroy(tempShotGo);
        DateTime NowTime = DateTime.Now.ToLocalTime();
        shot.name = "wpa_" + NowTime.Year + NowTime.Month + NowTime.Day + NowTime.Hour + NowTime.Minute + NowTime.Second;
        imgPath = CommonTool.SaveImages(shot);
        UITool.ShowToastMessage(this, "图片已保存\r\n" + imgPath, 35);
    }
    */
    //初始化文章内容
    public void SelectArticle(string title, string user, string content)
    {
        startShareLayer.SetActive(true);
        selectModeLayer.SetActive(true);
        wordNameText.text = title;
        dicNameText.text = user;
        dicText.text = content;

        StartCoroutine(SetHeight());
    }
    //初始化词典内容
    public void SelectDic(MatchedWordDetail word)
    {
        startShareLayer.SetActive(true);
        selectModeLayer.SetActive(true);
        wordNameText.text = word.word;
        dicNameText.text = word.dicName;
        dicText.text = word.meaning;

        StartCoroutine(SetHeight());

    }

    IEnumerator SetHeight()
    {
        yield return null;
        RectTransform rt = dicText.GetComponent<RectTransform>();
        float textHeight = rt.sizeDelta.y;
        rt.localPosition = new Vector3(rt.localPosition.x, -textHeight * 0.5f, rt.localPosition.z);

        float size = textHeight + 800;
        shareCompRT.sizeDelta = new Vector2(shareCompRT.sizeDelta.x, size);
        //shareCompRT.localPosition = new Vector3(shareCompRT.localPosition.x, -size * 0.5f, shareCompRT.localPosition.z);

        float sizeFill = size - 300;

        backImgBackRT.sizeDelta = new Vector2(backImgBackRT.sizeDelta.x, sizeFill);
        //backImgBackRT.position = new Vector3(backImgBackRT.position.x,/* -fillImgBaseHeight*/ - sizeFill * 0.5f, backImgBackRT.position.z);
        backImgBackRT.localPosition = new Vector3(backImgBackRT.localPosition.x, test, backImgBackRT.localPosition.z);

        backImgRT.sizeDelta = new Vector2(backImgRT.sizeDelta.x, sizeFill);
        backImgRT.localPosition = new Vector3(backImgRT.localPosition.x, test2, backImgRT.localPosition.z);
        SetBackImgMatParam(backImgRT.sizeDelta.x, backImgRT.sizeDelta.y);

    }
    void SetBackImgMatParam(float x, float y)
    {
        backImg.material.SetVector("_SizeXY", new Vector4(x, y, 1, 1));
        backImg.gameObject.SetActive(false);
        backImg.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ImgShot();
        }
    }

    #region 分享到平台
    void OnShareWeChat()
    {
        //  shareTool.CommonShare(PlatformType.WeChat, imgPath);
    }
    void OnShareWeChatMoments()
    {
        //  shareTool.CommonShare(PlatformType.WeChatMoments, imgPath);
    }
    void OnShareQQ()
    {
        // shareTool.CommonShare(PlatformType.QQ, imgPath);
    }
    void OnShareQZone()
    {
        // shareTool.CommonShare(PlatformType.QZone, imgPath);
    }
    void OnShareSina()
    {
        // shareTool.CommonShare(PlatformType.SinaWeibo, imgPath);
    }
    #endregion
}
