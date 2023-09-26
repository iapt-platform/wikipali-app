using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleManager;
using static DictManager;

public class DicGroupPopView : MonoBehaviour
{
    public PopViewType currViewType;
    public Text titleText;
    public Button popViewReturnBtn;
    public Button popViewCancelBtn;
    public Button okBtn;
    public InputField inputField;
    public ArticleGroupInfo articleGroupInfo;
    public DicGroupInfo dicGroupInfo;
    public DicGroupView dView;
    public bool isEdit = true;
    public void Init(DicGroupInfo _dicGroupInfo)
    {
        currViewType = PopViewType.SaveDic;
        dicGroupInfo = _dicGroupInfo;
        inputField.text = dicGroupInfo.groupName;
    }
    public void Init(ArticleGroupInfo _articleGroupInfo)
    {
        currViewType = PopViewType.SaveArticle;
        articleGroupInfo = _articleGroupInfo;
        inputField.text = articleGroupInfo.groupName;
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
    void ChangeNameDic()
    {
        dView.Init(PopViewType.SaveDic);
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
    void ChangeNameArticle()
    {
        dView.Init(PopViewType.SaveArticle);
        if (isEdit)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                //todo 弹出提示
            }
            else if (inputField.text != articleGroupInfo.groupName)
            {
                articleGroupInfo.groupName = inputField.text;
                ArticleManager.Instance().ChangeGroupName(articleGroupInfo.groupID, articleGroupInfo.groupName);
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
                ArticleManager.Instance().AddGroup(inputField.text);
                dView.RefreshGroupList();
            }
        }
        this.gameObject.SetActive(false);

    }
    //改名
    public void OnOkBtnClick()
    {
        if (currViewType == PopViewType.SaveDic)
        {
            ChangeNameDic();
        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            ChangeNameArticle();
        }

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
