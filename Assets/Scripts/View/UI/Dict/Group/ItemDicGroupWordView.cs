using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDicGroupWordView : MonoBehaviour
{
    public Text titleText;
    public Button delBtn;
    public Button textBtn;
    string word;
    int groupID;
    CommonGroupView commonView;
    public DicGroupView dicGroupView;
    public PopView popView;
    public void Init(string _word, int _groupID, CommonGroupView _commonView)
    {
        word = _word;
        commonView = _commonView;
        groupID = _groupID;
        titleText.text = word;
    }

    // Start is called before the first frame update
    void Start()
    {
        delBtn.onClick.AddListener(OnDelBtnClick);
        textBtn.onClick.AddListener(OnTextBtnClick);
    }
    public void OnDelBtnClick()
    {
        DictManager.Instance().DelWord(groupID, word);
        commonView.RefreshGroupList();
    }
    //跳转到查词
    public void OnTextBtnClick()
    {
        GameManager.Instance().mainView.SetDicOn();
        GameManager.Instance().mainView.dicView.OnItemDicClick(word);
        popView.OnCloseBackBtnClick();
        dicGroupView.OnCloseBtnClick();
        commonView.OnCloseBtnClick();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
