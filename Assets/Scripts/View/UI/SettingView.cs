using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    public Button aboutBtn;
    public Button updateBtn;
    public Text versionText;
    public Button transContentSliderBtn;
    public Slider transContentSliderToggle;
    public CommonGroupView commonGroupView;
    // Start is called before the first frame update
    void Start()
    {
        transContentSliderToggle.value = SettingManager.Instance().GetTransContent();
        transContentSliderToggle.onValueChanged.AddListener(OnTransContentToggleValueChanged);
        transContentSliderBtn.onClick.AddListener(OnTransContentBtnClick);
        aboutBtn.onClick.AddListener(OnAboutBtnClick);
        updateBtn.onClick.AddListener(OnUpdateBtnClick);
        versionText.text = "        v" + Application.version;
    }
    void OnTransContentToggleValueChanged(float value)
    {
        //Debug.LogError(value);
        SettingManager.Instance().SetTransContent((int)value);
    }
    void OnTransContentBtnClick()
    {
        if (transContentSliderToggle.value > 0.5f)
        {
            transContentSliderToggle.value = 0;
        }
        else
        {
            transContentSliderToggle.value = 1;
        }
    }
    void OnAboutBtnClick()
    {
        commonGroupView.InitAboutView();
        commonGroupView.gameObject.SetActive(true);
    }
    void OnUpdateBtnClick()
    {
        StartCoroutine(UpdateManager.Instance().Test());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
