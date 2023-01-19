using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ItemDicView : MonoBehaviour
{
    //单词列表面板
    public RectTransform SummaryScrollView;
    //单词详情面板
    public RectTransform DetailScrollView;
    public Button btn;
    public MatchedWord word;
    public Text nameTxt;
    public Text detailTxt;
    public DicView dicView;
    public void SetMeaning(MatchedWord matchedWord)
    {
        //解释限制1行
        string mean = new System.IO.StringReader(matchedWord.meaning).ReadLine();
        nameTxt.text = matchedWord.word;
        detailTxt.text = mean;
        word = matchedWord;
    }
    // Start is called before the first frame update
    void Start()
    {
        //btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnBtnClick);

    }
    public void OnBtnClick()
    {
        //SetSummaryOff();

        dicView.OnItemDicClick(word);
    }
    //void SetSummaryOff()
    //{
    //    DetailScrollView.gameObject.SetActive(true);
    //    SummaryScrollView.gameObject.SetActive(false);
    //}
    // Update is called once per frame
    void Update()
    {

    }
}
