using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareSelectPopView : MonoBehaviour
{
    public ItemShareDicSelectView itemView;
    public Button returnBtn;
    public Button okBtn;
    public ShareView shareView;
    List<string> dicIDStrList;
    List<string> dicNameStrList;
    public void Init(List<string> _dicIDStrList, List<string> _dicNameStrList)
    {
        dicIDStrList = _dicIDStrList;
        dicNameStrList = _dicNameStrList;
        RefreshItemList();

    }
    List<ItemShareDicSelectView> itemGoList = new List<ItemShareDicSelectView>();
    void DestroyItemList()
    {
        int l = itemGoList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemGoList[i]);
        }
        itemGoList.Clear();

    }
    void RefreshItemList()
    {
        DestroyItemList();
        int l = dicIDStrList.Count;
        for (int i = 0; i < l; i++)
        {
            GameObject inst = Instantiate(itemView.gameObject, itemView.transform.parent, false);
            inst.transform.position = itemView.transform.position;
            ItemShareDicSelectView view = inst.GetComponent<ItemShareDicSelectView>();
            view.Init(dicIDStrList[i], dicNameStrList[i]);
            inst.SetActive(true);
            itemGoList.Add(view);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        returnBtn.onClick.AddListener(OnCloseBackBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);
    }
    public void OnCloseBackBtnClick()
    {
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }
    public void OnOkBtnClick()
    {
        int l = itemGoList.Count;
        for (int i = 0; i < l; i++)
        {
            if (itemGoList[i].toggle.isOn)
            {

                shareView.SelectDic(dicIDStrList[i], dicNameStrList[i]);
                break;
            }

        }

        this.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
