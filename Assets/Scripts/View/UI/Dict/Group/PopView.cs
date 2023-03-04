using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopView : MonoBehaviour
{
    public Button editGroupBtn;
    public Button closeBackGroupBtn;
    public Button okBtn;
    public DicGroupView dicGroupView;
    public ItemDicGroupPopView groupItem;
    // Start is called before the first frame update
    void Start()
    {
        editGroupBtn.onClick.AddListener(OnEditBtnClick);
        closeBackGroupBtn.onClick.AddListener(OnCloseBackBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);

    }
    public void OnEditBtnClick()
    {
        dicGroupView.gameObject.SetActive(true);
    }
    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void OnCloseBackBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void OnOkBtnClick()
    {

        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 刷新分组信息
    /// </summary>
    void RefreshGroupList()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
