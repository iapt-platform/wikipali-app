using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;
/// <summary>
/// 词典面板
/// </summary>
public class DicView : MonoBehaviour
{
    public Button searchBtn;
    //用户输入的查词
    public InputField userInput;
    public Button itemDicBtn;
    public RectTransform scrollContent;
    //todo 单例模式
    public DictManager dicManager;

    public void OnSearchBtnClick()
    {
        Debug.LogError("你单击了Button");
        //SqliteDataReader reader = dbManager.db.SelectOrderASC("bh-paper", "word");
        SearchWord(userInput.text);

    }
    void SearchWord(string inputStr)
    {
        MatchedWord[] matchedWordArr = dicManager.MatchWord(inputStr);
        int length = matchedWordArr.Length;
        float height = userInput.flexibleHeight;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(itemDicBtn.gameObject, scrollContent);
            inst.transform.position = itemDicBtn.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            //解释限制1行
            string mean = new System.IO.StringReader(matchedWordArr[i].meaning).ReadLine();
            //matchedWordArr[i].meaning.Substring(0, matchedWordArr[i].meaning.IndexOf(System.Environment.NewLine));
            //string first = new StringReader(str).ReadLine();
            inst.GetComponent<ItemDicView>().SetMeaning(matchedWordArr[i].word, mean);
            inst.SetActive(true);
        }
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, height * length);

    }
    // Start is called before the first frame update
    void Start()
    {
        searchBtn.onClick.AddListener(OnSearchBtnClick);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
