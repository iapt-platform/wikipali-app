using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ItemDicGroupEditView : MonoBehaviour
{
    public DicGroupPopView dicGroupPopView;
    public Text titleText;
    public Button wordBtn;
    public Button editBtn;
    public Button delBtn;
    public DicGroupInfo dicGroupInfo;
    public DicGroupView dView;
    public CommonGroupView commonGroupView;
    public void Init(DicGroupInfo _dicGroupInfo)
    {
        dicGroupInfo = _dicGroupInfo;
        titleText.text = dicGroupInfo.groupName;
    }
    // Start is called before the first frame update
    void Start()
    {
        editBtn.onClick.AddListener(OnEditBtnClick);
        delBtn.onClick.AddListener(OnDelBtnClick);
        wordBtn.onClick.AddListener(OnWordBtnClick);

    }

    public void OnEditBtnClick()
    {
        //todo：回调函数
        dicGroupPopView.Init(dicGroupInfo);
        dicGroupPopView.SetEdit();
        dicGroupPopView.gameObject.SetActive(true);

    }
    void DelFunc(bool boolean)
    {
        if (boolean)
        {
            DictManager.Instance().DelGroup(dicGroupInfo.groupID);
            dView.RefreshGroupList();
        }
    }
    public void OnDelBtnClick()
    {
        //todo:
        UITool.PopOutView(DelFunc);

    }
    public void OnWordBtnClick()
    {
        commonGroupView.InitDicGroupWordView(dicGroupInfo);
        commonGroupView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
