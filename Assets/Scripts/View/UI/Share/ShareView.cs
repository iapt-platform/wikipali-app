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
    public Button backBtn;

    public void Init()
    {
        startShareLayer.SetActive(false);
        shareBtnsLayer.SetActive(false);
        selectModeLayer.SetActive(false);
        captureImgLayer.SetActive(false);
        selectDicPopLayer.gameObject.SetActive(true);
        selectDicPopLayer.Init();
    }
    // Start is called before the first frame update
    void Awake()
    {
        fillImgBaseHeight = backImgBackRT.localPosition.y - backImgBackRT.sizeDelta.y * 0.5f;
        test = backImgBackRT.localPosition.y;
        test2 = backImgRT.localPosition.y;

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
    public void OnModeBtnClick(int id)
    {
        //Debug.LogError(id);
        backImg.material = backImgMatArr[id];
        SetBackImgMatParam(backImgRT.sizeDelta.x, backImgRT.sizeDelta.y);

    }
    // Update is called once per frame
    void Update()
    {

    }

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
}
