using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicGroupPopView : MonoBehaviour
{
    public Button popViewReturnBtn;
    public Button popViewCancelBtn;
    public Button okBtn;

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
    public void OnOkBtnClick()
    {

        this.gameObject.SetActive(false);
    }
}
