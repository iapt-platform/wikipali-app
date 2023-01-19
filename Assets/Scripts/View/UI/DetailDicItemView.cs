using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class DetailDicItemView : MonoBehaviour
{
    public Button titleBtn;
    public Image dropDownImg;
    public Text titleTxt;
    public Text detailTxt;
    public DicView dicView;
    MatchedWordDetail word;
    public void Init(MatchedWordDetail wordDic)
    {
        word = wordDic;
        titleTxt.text = word.dicName;
        string detail = word.meaning;
        //部分高亮
        detail = detail.Replace("【", "<color=blue>【");
        detail = detail.Replace("】", "】</color>");
        detailTxt.text = detail;
        LayoutRebuilder.ForceRebuildLayoutImmediate(detailTxt.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(dicView.detailScrollContent);
    }
    public float GetHeight()
    {
        float height = titleBtn.GetComponent<RectTransform>().sizeDelta.y;
        height += detailTxt.GetComponent<RectTransform>().sizeDelta.y;
        return height;
    }
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
