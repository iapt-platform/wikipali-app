using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopArticleSentenceSelectView : MonoBehaviour
{
    public ShareView shareView;
    public ItemArticleSentenceView itemTemp;
    public Button selectAllBtn;
    public Button returnBtn;
    public Button okBtn;
    // Start is called before the first frame update
    void Start()
    {
        selectAllBtn.onClick.AddListener(OnSelectAllBtnClick);
        returnBtn.onClick.AddListener(OnReturnBtnClick);
        okBtn.onClick.AddListener(OnOkBtnClick);
    }
    public void OnSelectAllBtnClick()
    { 
    
    }
    public void OnReturnBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void OnOkBtnClick()
    {
        shareView.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void RefreshGroupList()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
