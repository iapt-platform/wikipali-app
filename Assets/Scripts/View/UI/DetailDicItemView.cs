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
    public float itemHeight;
    //是否是折叠状态
    bool isFolded = false;
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

    public void OnBtnClick()
    {
        if (isFolded)
        {
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta  = new Vector2(size.x, itemHeight);
            detailTxt.gameObject.SetActive(true);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            dropDownImg.rectTransform.localRotation = Quaternion.Euler(0, 0, 180);
            isFolded = false;
        }
        else
        {
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, titleBtn.GetComponent<RectTransform>().sizeDelta.y);
            detailTxt.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            dropDownImg.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            isFolded = true;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        titleBtn.onClick.AddListener(OnBtnClick);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
