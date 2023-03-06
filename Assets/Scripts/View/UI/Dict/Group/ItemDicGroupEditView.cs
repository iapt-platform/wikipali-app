using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ItemDicGroupEditView : MonoBehaviour
{
    public DicGroupPopView dicGroupPopView;
    public Text titleText;
    public Button editBtn;
    public Button delBtn;
    public DicGroupInfo dicGroupInfo;
    public void Init(DicGroupInfo _dicGroupInfo)
    {
        dicGroupInfo = _dicGroupInfo;
        titleText.text = dicGroupInfo.groupName;
    }
    // Start is called before the first frame update
    void Start()
    {
        editBtn.onClick.AddListener(OnEditBtnClick);
        delBtn.onClick.AddListener(OnEditBtnClick);

    }

    public void OnEditBtnClick()
    {
        dicGroupPopView.Init(dicGroupInfo);
        dicGroupPopView.gameObject.SetActive(true);

    }
    public void OnDelBtnClick()
    {
        //todo:
        UITool.PopOutView();
        DictManager.Instance().DelGroup(dicGroupInfo.groupID);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
