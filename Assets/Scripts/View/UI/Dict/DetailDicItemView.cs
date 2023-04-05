using Hypertext;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class DetailDicItemView : MonoBehaviour
{
    public Button titleBtn;
    public Image dropDownImg;
    public Text titleTxt;
    public RegexHypertext detailTxt;
    public DicView dicView;
    public PopTermView popTView;
    MatchedWordDetail word;
    public float itemHeight;
    //是否是折叠状态
    bool isFolded = false;
    const int TEXT_MAX = 15000;
    List<Text> splitTextList = new List<Text>();
    public void Init(MatchedWordDetail wordDic)
    {
        word = wordDic;
        titleTxt.text = word.dicName;
        string detail = word.meaning;
        //部分高亮
        //detail = detail.Replace("【", "<color=blue>【");
        //detail = detail.Replace("】", "】</color>");
        //解决文字过长 text报错 的问题，实例化多个text ArgumentException: Mesh can not have more than 65000 vertices
        int textCount = Mathf.CeilToInt((float)detail.Length / (float)TEXT_MAX);
        if (textCount > 1)
        {
            string detailTemp = detail;
            float height = 0;
            for (int i = 0; i < textCount; i++)
            {
                int min = i * TEXT_MAX;
                int max = (i + 1) * TEXT_MAX;
                max = max > (detail.Length - 1) ?
                    (detail.Length - 1) : max;
                int l = max - min;
                //todo:英文单词分开换行接续问题
                string temp = detailTemp.Substring(min, l);
                if (i != 0)
                {
                    temp = "-" + temp;
                }
                if (i != textCount - 1)
                {
                    temp = temp + "-";
                }
                GameObject inst = Instantiate(detailTxt.gameObject, detailTxt.transform.parent, false);
                inst.name = i.ToString();
                inst.transform.position = detailTxt.transform.position;
                Text contentTextInst = inst.GetComponent<Text>();
                contentTextInst.text = temp;
                contentTextInst.rectTransform.position = detailTxt.rectTransform.position;
                //contentTextInst.rectTransform.position -= Vector3.up * height;
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentTextInst.rectTransform);
                //height += contentTextInst.rectTransform.sizeDelta.y;
                splitTextList.Add(contentTextInst);
            }
            //detailTxt.text = "";
            detailTxt.gameObject.SetActive(false);
        }
        else
        {
            detailTxt.text = detail;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(detailTxt.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(dicView.detailScrollContent);

    }
    public float GetHeight()
    {
        float height = titleBtn.GetComponent<RectTransform>().sizeDelta.y;
        if (detailTxt.gameObject.activeSelf)
        {
            height += detailTxt.GetComponent<RectTransform>().sizeDelta.y;
        }
        else
        {
            float splitTextsHeight = 0;
            for (int i = 0; i < splitTextList.Count; i++)
            {
                Text contentTextInst = splitTextList[i];
                //contentTextInst.transform.position -= Vector3.up * splitTextsHeight;
                //Debug.LogError(contentTextInst.rectTransform.localPosition);
                contentTextInst.rectTransform.localPosition -= Vector3.up * splitTextsHeight;
                //Debug.LogError(Vector3.up * splitTextsHeight);
                //Debug.LogError(contentTextInst.rectTransform.localPosition);
                //Debug.LogError("----------------------");
                splitTextsHeight += contentTextInst.rectTransform.sizeDelta.y + height;
            }
            height += splitTextsHeight;
        }
        return height;
    }

    public void OnBtnClick()
    {
        if (isFolded)
        {
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, itemHeight);
            if (splitTextList != null && splitTextList.Count > 0)
            {
                for (int i = 0; i < splitTextList.Count; i++)
                {
                    splitTextList[i].gameObject.SetActive(true);
                }
            }
            else
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
            if (splitTextList != null && splitTextList.Count > 0)
            {
                for (int i = 0; i < splitTextList.Count; i++)
                {
                    splitTextList[i].gameObject.SetActive(false);
                }

            }
            else
                detailTxt.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            dropDownImg.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            isFolded = true;
        }

    }
    const string RegexHashChineseTag = @"【(\w+)】";
    //const string RegexHashChineseTag = @"[0-9]";

    // Start is called before the first frame update
    void Start()
    {
        titleBtn.onClick.AddListener(OnBtnClick);
        //detailTxt.OnClick(RegexHashChineseTag, Color.blue, hashtag => RegexDicDetailView(hashtag));
        detailTxt.OnClick(RegexHypertext.GRM_ABBR_TAG, Color.blue, hashtag => RegexDicDetailView(hashtag));
    }
    void RegexDicDetailView(string content)
    {
        string value = UserGrammar.grm_abbr[content];
        //string[] test = UserGrammar.grammar_cn["grammar_nt"];
        string[] test = UserGrammar.grammar_cn["grammar_" + value];
        StringBuilder sb = new StringBuilder();
        int l = test.Length;
        for (int i = 0; i < l; i++)
        {
            sb.AppendLine(test[i]);
        }
        popTView.Init(test[0], sb.ToString());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
