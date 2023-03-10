using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class DicGroupPopView : MonoBehaviour
{
    public Text titleText;
    public Button popViewReturnBtn;
    public Button popViewCancelBtn;
    public Button okBtn;
    public InputField inputField;
    public DicGroupInfo dicGroupInfo;
    public DicGroupView dView;
    public bool isEdit = true;
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
        if (isEdit)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                //todo 弹出提示
            }
            else if (inputField.text != dicGroupInfo.groupName)
            {
                dicGroupInfo.groupName = inputField.text;
                DictManager.Instance().ChangeGroupName(dicGroupInfo.groupID, dicGroupInfo.groupName);
                dView.RefreshGroupList();
            }
        }
        else
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                //todo 弹出提示
            }
            else
            {
                DictManager.Instance().AddGroup(inputField.text);
                dView.RefreshGroupList();
            }
        }
        this.gameObject.SetActive(false);
    }
    //编辑模式
    public void SetEdit()
    {
        isEdit = true;
        titleText.text = "编辑笔记本";
    }
    //增加模式
    public void SetAdd()
    {
        isEdit = false;
        titleText.text = "新建笔记本";
        inputField.text = "";
    }
}
