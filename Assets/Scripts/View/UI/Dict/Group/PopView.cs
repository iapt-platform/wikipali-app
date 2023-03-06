using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class PopView : MonoBehaviour
{
    public Button editGroupBtn;
    public Button closeBackGroupBtn;
    public Button okBtn;
    public DicGroupView dicGroupView;
    public ItemDicGroupPopView groupItem;
    //todo??????

    public string currWord;
    // Start is called before the first frame update
    void Start()
    {
        editGroupBtn.onClick.AddListener(OnEditBtnClick);
        closeBackGroupBtn.onClick.AddListener(OnCloseBackBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);

    }
    public void OnEditBtnClick()
    {
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void OnCloseBackBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void OnOkBtnClick()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            if (itemList[i].GetSelectState())
            {
                if (!itemList[i].dicGroupInfo.wordList.Contains(currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Add(currWord);
                }
            }
            else
            {
                if (itemList[i].dicGroupInfo.wordList.Contains(currWord))
                {
                    itemList[i].dicGroupInfo.wordList.Remove(currWord);
                }
            }
        }
        this.gameObject.SetActive(false);
        DictManager.Instance().ModifyDicGroup();
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
