using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareView : MonoBehaviour
{

    [Header("控件部分")]
    public GameObject StartShareLayer;
    public GameObject SelectShareLayer;

    //组成图片的部分
    [Header("组成图片的部分")]
    public Text wordNameText;
    public Text dicNameText;
    public Text dicText;
    public Image backImg;
    public Material[] backImgMatArr;


    //分享的部分
    [Header("分享的部分")]
    public Button returnBtn;
    public Button shareWeiXin;
    public Button shareWeiXinPYQ;
    public Button shareQQ;
    public Button shareQQKJ;
    public Button shareSina;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectDic(string dicID,string dicName)
    {
        SelectShareLayer.SetActive(true);
        SetDicImageInfo("","","");
    }
    public void SetDicImageInfo(string word,string dicName,string dicDesc)
    {
        wordNameText.text = word;
        dicNameText.text = dicName;
        dicText.text = dicDesc;
    }


}
