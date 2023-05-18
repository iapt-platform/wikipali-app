using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGroupArticleView : MonoBehaviour
{
    public Toggle starToggle;
    public Button shareBtn;
    public PopArticleSentenceSelectView popSentenceSelectView;
    public PopView popView;
    //public ShareView shareView;
    // Start is called before the first frame update
    void Awake()
    {
        starToggle.onValueChanged.AddListener(OnToggleValueChanged);
        shareBtn.onClick.AddListener(OnShareBtnClick);

    }
    public void OnShareBtnClick()
    {
        popSentenceSelectView.Init();
        popSentenceSelectView.gameObject.SetActive(true);

        //shareView.gameObject.SetActive(true);
        //shareView.Init();
    }
    bool isSet = false;
    public void SetToggleValue(bool isOn)
    {
        //bug:??????????第一次进收藏过的单词，点击星后，没有触发OnToggleValueChanged
        //已解决：注册事件改为Awake就好了，事件还未注册就设置toggle了
        if (starToggle.isOn != isOn)
        {
            isSet = true;
            starToggle.isOn = isOn;
        }
    }
    void OnToggleValueChanged(bool value)
    {
        if (isSet)
        {
            isSet = false;
            return;
        }
        popView.Init(PopViewType.SaveArticle);
        popView.RefreshGroupList();
        popView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
