using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ArticleManager;
using static DictManager;

public class ItemDicGroupEditView : MonoBehaviour
{
    public PopViewType currViewType;
    public DicGroupPopView dicGroupPopView;
    public Text titleText;
    public Button wordBtn;
    public Button editBtn;
    public Button delBtn;
    public DicGroupInfo dicGroupInfo;
    public ArticleGroupInfo articleGroupInfo;
    public DicGroupView dView;
    public CommonGroupView commonGroupView;
    public void Init(DicGroupInfo _dicGroupInfo)
    {
        dicGroupInfo = _dicGroupInfo;
        titleText.text = dicGroupInfo.groupName;
        currViewType = PopViewType.SaveDic;
    }
    public void Init(ArticleGroupInfo _articleGroupInfo)
    {
        articleGroupInfo = _articleGroupInfo;
        titleText.text = articleGroupInfo.groupName;
        currViewType = PopViewType.SaveArticle;
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
        if (currViewType == PopViewType.SaveDic)
        {
            dicGroupPopView.Init(dicGroupInfo);

        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            dicGroupPopView.Init(articleGroupInfo);
        }
        dicGroupPopView.SetEdit();
        dicGroupPopView.gameObject.SetActive(true);

    }
    void DelFunc(bool boolean)
    {
        if (boolean)
        {
            if (currViewType == PopViewType.SaveDic)
            {
                DictManager.Instance().DelGroup(dicGroupInfo.groupID);
                dView.Init(PopViewType.SaveDic);
            }
            else if (currViewType == PopViewType.SaveArticle)
            {
                ArticleManager.Instance().DelGroup(articleGroupInfo.groupID);
                dView.Init(PopViewType.SaveArticle);
            }
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
        if (currViewType == PopViewType.SaveDic)
        {
            commonGroupView.InitDicGroupWordView(dicGroupInfo);


        }
        else if (currViewType == PopViewType.SaveArticle)
        {
            commonGroupView.InitArticleGroupWordView(articleGroupInfo);

        }
        commonGroupView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
