using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static DictManager;
/// <summary>
/// 词典面板
/// </summary>
public class DicView : MonoBehaviour
{
    public Button searchBtn;
    public Button delBtn;
    //用户输入的查词
    public InputField userInput;
    public Button itemDicBtn;
    public RectTransform scrollContent;
    //单词列表面板
    public RectTransform SummaryScrollView;
    //单词详情面板
    public RectTransform DetailScrollView;
    //todo 单例模式
    public DictManager dicManager;
    public bool isDelBtnOn = false;
    public void OnSearchInputClick()
    {


    }

    public void OnDelBtnClick()
    {
        SetSummaryOn();
        userInput.text = "";
    }
    public void OnSearchBtnClick()
    {
        //Debug.LogError("你单击了Button");
        DestroyItemDicList();
        //SqliteDataReader reader = dbManager.db.SelectOrderASC("bh-paper", "word");
        SearchWord(userInput.text);

    }
    public void OnSearchValueChanged(string value)
    {
        //Debug.LogError(value);
        DestroyItemDicList();
        SearchWord(userInput.text);
    }
    List<GameObject> itemDicList = new List<GameObject>();
    void SearchWord(string inputStr)
    {
        if (string.IsNullOrEmpty(inputStr))
        {
            SetDelBtn(false);
            return;
        }
        SetDelBtn(true);
        MatchedWord[] matchedWordArr = dicManager.MatchWord(inputStr);
        int length = matchedWordArr.Length;
        float height = itemDicBtn.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(itemDicBtn.gameObject, scrollContent);
            inst.transform.position = itemDicBtn.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height * i;

            //matchedWordArr[i].meaning.Substring(0, matchedWordArr[i].meaning.IndexOf(System.Environment.NewLine));
            //string first = new StringReader(str).ReadLine();
            inst.GetComponent<ItemDicView>().SetMeaning(matchedWordArr[i]);
            inst.SetActive(true);
            itemDicList.Add(inst);
        }
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, height * length);

    }
    //销毁下拉列表GO
    private void DestroyItemDicList()
    {
        int length = itemDicList.Count;
        if (length == 0)
            return;
        for (int i = 0; i < length; i++)
        {
            Destroy(itemDicList[i]);
        }
        itemDicList.Clear();
    }
    //设置删除输入文字按钮开关
    void SetDelBtn(bool sw)
    {
        if (sw != isDelBtnOn)
        {
            isDelBtnOn = sw;
            delBtn.gameObject.SetActive(sw);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AddInputNameClickEvent();
        delBtn.onClick.AddListener(OnDelBtnClick);
        //userInput.OnPointerClick.AddListener(OnSearchInputClick);
        userInput.onValueChanged.AddListener(OnSearchValueChanged);
    }
    private void AddInputNameClickEvent() //可以在Awake中调用
    {
        var eventTrigger = userInput.GetComponent<EventTrigger>();
        UnityAction<BaseEventData> selectEvent = OnSearchInputFieldClicked;
        EventTrigger.Entry onClick = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick
        };

        onClick.callback.AddListener(selectEvent);
        eventTrigger.triggers.Add(onClick);
    }

    private void OnSearchInputFieldClicked(BaseEventData data)
    {
        SetSummaryOn();
        //Debug.LogError("ddd");
    }
    void SetSummaryOn()
    {
        DetailScrollView.gameObject.SetActive(false);
        SummaryScrollView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
