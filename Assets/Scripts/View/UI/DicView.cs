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
    public DetailDicItemView detailDicItem;
    public RectTransform summaryScrollContent;
    public RectTransform detailScrollContent;
    //单词列表面板
    public RectTransform SummaryScrollView;
    //单词详情面板
    public RectTransform DetailScrollView;
    //todo 单例模式
    public DictManager dicManager = DictManager.Instance();
    public bool isDelBtnOn = false;
    //是否是补全单词，补全不是用户输入
    public bool isComplement = false;

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
        if (isComplement)
            return;
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
        //由于混入多个词典与英文和pali问查找结果，个数没做限制，在此处做限制
        int length = matchedWordArr.Length > DictManager.LIMIT_COUNT ? LIMIT_COUNT : matchedWordArr.Length;
        float height = itemDicBtn.GetComponent<RectTransform>().sizeDelta.y;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(itemDicBtn.gameObject, summaryScrollContent);
            inst.transform.position = itemDicBtn.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height * i;

            //matchedWordArr[i].meaning.Substring(0, matchedWordArr[i].meaning.IndexOf(System.Environment.NewLine));
            //string first = new StringReader(str).ReadLine();
            inst.GetComponent<ItemDicView>().SetMeaning(matchedWordArr[i]);
            inst.SetActive(true);
            itemDicList.Add(inst);
        }
        summaryScrollContent.sizeDelta = new Vector2(summaryScrollContent.sizeDelta.x, height * length);

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
    void SetSummaryOff()
    {
        DetailScrollView.gameObject.SetActive(true);
        SummaryScrollView.gameObject.SetActive(false);
    }
    /// <summary>
    /// 点击某个单词查询
    /// </summary>
    /// <param name="word"></param>
    public void OnItemDicClick(MatchedWord word)
    {
        SetSummaryOff();
        //TODO 根据查到的word的id和dicID查询，而不是直接用word全查？？？？但是太麻烦，，，要改 有多个相同词条存在
        DisplayWordDetail(word.word);
    }
    List<GameObject> detailDicItemList = new List<GameObject>();
    void DisplayWordDetail(string word)
    {
        DestroyDetailDicItemList();
        if (string.IsNullOrEmpty(word))
        {
            SetDelBtn(false);
            return;
        }
        //补全查词
        isComplement = true;
        userInput.text = word;
        isComplement = false;

        MatchedWordDetail[] matchedWordArr = dicManager.MatchWordDetail(word);
        int length = matchedWordArr.Length;
        float height = 0;
        for (int i = 0; i < length; i++)
        {
            GameObject inst = Instantiate(detailDicItem.gameObject, detailScrollContent);
            inst.transform.position = detailDicItem.transform.position;
            inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            DetailDicItemView ddiv = inst.GetComponent<DetailDicItemView>();
            ddiv.Init(matchedWordArr[i]);
            height += ddiv.GetHeight();
            inst.SetActive(true);
            detailDicItemList.Add(inst);
        }
        //等下一帧UI刷新后获取位置
        StartCoroutine(SetHeight());

    }
    IEnumerator SetHeight()
    {
        yield return null;
        int length = detailDicItemList.Count;
        float height = 0;
        for (int i = 0; i < length; i++)
        {
            detailDicItemList[i].transform.position = detailDicItem.transform.position;
            detailDicItemList[i].GetComponent<RectTransform>().position -= Vector3.up * height;
            DetailDicItemView ddiv = detailDicItemList[i].GetComponent<DetailDicItemView>();
            //?为啥会缩100？
            height += ddiv.GetHeight() + 200;
        }
        detailScrollContent.sizeDelta = new Vector2(detailScrollContent.sizeDelta.x, height);

    }
    //销毁词典列表GO
    private void DestroyDetailDicItemList()
    {
        int length = detailDicItemList.Count;
        if (length == 0)
            return;
        for (int i = 0; i < length; i++)
        {
            Destroy(detailDicItemList[i]);
        }
        detailDicItemList.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
