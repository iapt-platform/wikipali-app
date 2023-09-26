using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherWordItemView : MonoBehaviour
{
    public Button titleBtn;
    public Image dropDownImg;
    public Text titleTxt;
    public GameObject wordItemParentGo;
    public GameObject wordItemGo;
    public Text dicText;
    public Text typeText;
    public DicView dicView;
    Dictionary<string, List<string>> caseWordList;
    float itemHeight;
    float allHeight;
    //是否是折叠状态
    bool isFolded = false;
    public void Awake()
    {
        itemHeight = dicText.GetComponent<RectTransform>().sizeDelta.y;
        itemHeight += typeText.GetComponent<RectTransform>().sizeDelta.y;
        //?????????
        itemHeight = 120;
    }
    //public Text wordBtn;
    public float GetHeight()
    {
        float height = titleBtn.GetComponent<RectTransform>().sizeDelta.y;
        allHeight = height + itemHeight * caseWordList.Count + 10;
        return allHeight;
    }
    public void Init(Dictionary<string, List<string>> _caseWordList)
    {
        caseWordList = _caseWordList;
        if (caseWordList.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
            gameObject.SetActive(true);
        RefreshList();
        float height = GetHeight();
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }
    public void SetLayerEnd()
    {
        this.GetComponent<RectTransform>().SetAsLastSibling();
    }
    List<GameObject> wordItemList = new List<GameObject>();
    void DestroyList()
    {
        int c = wordItemList.Count;
        for (int i = 0; i < c; i++)
        {
            Destroy(wordItemList[i]);
        }
        wordItemList.Clear();
    }
    void RefreshList()
    {
        DestroyList();
        int id = 0;
        foreach (KeyValuePair<string, List<string>> kvp in caseWordList)
        {
            GameObject obj = Instantiate(wordItemGo, wordItemGo.transform.parent, false);
            obj.SetActive(true);
            obj.GetComponent<RectTransform>().localPosition += Vector3.down * id * itemHeight;
            Text word = obj.GetComponent<Text>();
            Button wordBtn = obj.GetComponent<Button>();
            wordBtn.onClick.AddListener(() => OnWordClick(kvp.Key));
            word.text = kvp.Key;
            Text type = obj.transform.GetChild(0).GetComponent<Text>();
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                type.text += kvp.Value[i].Replace("$."," ") + "|";
            }
            wordItemList.Add(obj);
            ++id;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        titleBtn.onClick.AddListener(OnBtnClick);

    }
    public void OnBtnClick()
    {
        if (isFolded)
        {
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, allHeight);
            wordItemParentGo.gameObject.SetActive(true);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            dropDownImg.rectTransform.localRotation = Quaternion.Euler(0, 0, 180);
            isFolded = false;
        }
        else
        {
            Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, titleBtn.GetComponent<RectTransform>().sizeDelta.y);
            wordItemParentGo.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            dropDownImg.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            isFolded = true;
        }

    }
    public void OnWordClick(string word)
    {
        //查询点中单词
        dicView.SetSummaryText(word);
    }
}
