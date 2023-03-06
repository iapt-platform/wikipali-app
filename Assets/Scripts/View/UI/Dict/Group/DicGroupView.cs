using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class DicGroupView : MonoBehaviour
{
    public Button returnBtn;

    //DicGroupPopView
    public DicGroupPopView dicGroupPopView;
    public ItemDicGroupEditView editItem;

    // Start is called before the first frame update
    void Start()
    {
        returnBtn.onClick.AddListener(OnCloseBtnClick);

    }
    List<ItemDicGroupEditView> itemList = new List<ItemDicGroupEditView>();
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
            GameObject inst = Instantiate(editItem.gameObject, editItem.transform.parent, false);
            inst.transform.position = editItem.transform.position;
            //inst.GetComponent<RectTransform>().position -= Vector3.up * height;
            ItemDicGroupEditView iv = inst.GetComponent<ItemDicGroupEditView>();
            iv.Init(allDicGroup[i]);
            inst.SetActive(true);
            itemList.Add(iv);
        }

    }
    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
