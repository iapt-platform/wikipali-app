using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ShareSelectPopView : MonoBehaviour
{
    public ItemShareDicSelectView itemView;
    public Button returnBtn;
    public Button okBtn;
    public ShareView shareView;
    MatchedWordDetail[] currMatchedWordDetail;
    public void Init()
    {
        currMatchedWordDetail = DictManager.Instance().currMatchedWordDetail;
        if (currMatchedWordDetail == null || currMatchedWordDetail.Length == 0)
        {
            shareView.gameObject.SetActive(false);
            return;
        }
        RefreshItemList();

    }
    List<ItemShareDicSelectView> itemGoList = new List<ItemShareDicSelectView>();
    void DestroyItemList()
    {
        int l = itemGoList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemGoList[i].gameObject);
        }
        itemGoList.Clear();

    }
    void RefreshItemList()
    {
        DestroyItemList();
        int l = currMatchedWordDetail.Length;
        for (int i = 0; i < l; i++)
        {
            GameObject inst = Instantiate(itemView.gameObject, itemView.transform.parent, false);
            inst.transform.position = itemView.transform.position;
            ItemShareDicSelectView view = inst.GetComponent<ItemShareDicSelectView>();
            view.Init(currMatchedWordDetail[i]);
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

                shareView.SelectDic(currMatchedWordDetail[i]);
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
