using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserView : MonoBehaviour
{
    public Button dicBtn;
    public Button articleBtn;
    public CommonGroupView commonGroupView;
    public DicGroupView dicGroupView;
    // Start is called before the first frame update
    void Start()
    {
        dicBtn.onClick.AddListener(OnDicBtnClick);
        articleBtn.onClick.AddListener(OnArticleBtnClick);
    }
    public void OnDicBtnClick()
    {
        dicGroupView.Init(PopViewType.SaveDic);
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    public void OnArticleBtnClick()
    {
        dicGroupView.Init(PopViewType.SaveArticle);
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
