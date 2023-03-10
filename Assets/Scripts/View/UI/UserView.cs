using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserView : MonoBehaviour
{
    public Button dicBtn;
    public CommonGroupView commonGroupView;
    public DicGroupView dicGroupView;
    // Start is called before the first frame update
    void Start()
    {
        dicBtn.onClick.AddListener(OnDicBtnClick);

    }
    public void OnDicBtnClick()
    {
        dicGroupView.RefreshGroupList();
        dicGroupView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
