using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;
public enum PopViewType
{
    SaveDic = 1,
    SaveArticle = 2,
}
public class PopView : MonoBehaviour
{

    public PopViewType currViewType;
    public Button editGroupBtn;
    public Button closeBackGroupBtn;
    public Button okBtn;
    public DicGroupView dicGroupView;
    public ItemDicGroupPopView groupItem;
    // Start is called before the first frame update
    void Start()
    {
        editGroupBtn.onClick.AddListener(OnEditBtnClick);
        closeBackGroupBtn.onClick.AddListener(OnCloseBackBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);

    }
    public void Init(PopViewType pvt)
    {
        currViewType = pvt;
    }
    public void OnEditBtnClick()
    {
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    //public void OnCloseBtnClick()
    //{
    //    DictManager.Instance().SetWordStar(DictManager.Instance().currWord);
    //    this.gameObject.SetActive(false);
    //}
    public void OnCloseBackBtnClick()
    {
        DictManager.Instance().SetWordStar(DictManager.Instance().currWord);
        this.gameObject.SetActive(false);
    }
    public void OnOkBtnClick()
    {
        int l = itemList.Count;
        bool isDirty = false;
        for (int i = 0; i < l; i++)
        {
            if (itemList[i].GetSelectState())
            {
                if (!itemList[i].dicGroupInfo.wordList.Contains(DictManager.Instance().currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Add(DictManager.Instance().currWord);
                    isDirty = true;
                }
            }
            else
            {
                if (itemList[i].dicGroupInfo.wordList.Contains(DictManager.Instance().currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Remove(DictManager.Instance().currWord);
                    isDirty = true;
                }
            }
        }
        this.gameObject.SetActive(false);
        if (isDirty)
            DictManager.Instance().ModifyDicGroup();
        DictManager.Instance().SetWordStar(DictManager.Instance().currWord);
    }
    List<ItemDicGroupPopView> itemList = new List<ItemDicGroupPopView>();
    /// <summary> 
    /// 刷新分组信息
    /// </summary>
    public void RefreshGroupList()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();

        List<DicGroupInfo> allDicGroup = DictManager.Instance().allDicGroup;
        int gl = allDicGroup.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(groupItem.gameObject, groupItem.transform.parent, false);
            inst.transform.position = groupItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupPopView iv = inst.GetComponent<ItemDicGroupPopView>();
            iv.Init(allDicGroup[i]);
            inst.SetActive(true);
            itemList.Add(iv);
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
