using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class DicGroupPopView : MonoBehaviour
{
    public Button popViewReturnBtn;
    public Button popViewCancelBtn;
    public Button okBtn;
    public InputField inputField;
    public DicGroupInfo dicGroupInfo;
    public void Init(DicGroupInfo _dicGroupInfo)
    {
        dicGroupInfo = _dicGroupInfo;
        inputField.text = dicGroupInfo.groupName;
    }
    // Start is called before the first frame update
    void Start()
    {
        popViewReturnBtn.onClick.AddListener(OnPopCloseBtnClick);
        popViewCancelBtn.onClick.AddListener(OnPopCloseBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);

    }
    public void OnPopCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    //改名
    public void OnOkBtnClick()
    {
        if(inputField.text != dicGroupInfo.groupName)
        {
            dicGroupInfo.groupName = inputField.text;
            DictManager.Instance().ChangeGroupName(dicGroupInfo.groupID, dicGroupInfo.groupName);
        }
        this.gameObject.SetActive(false);
    }
}
