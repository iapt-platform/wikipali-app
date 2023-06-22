using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SettingManager;
using static UpdateManager;

public class SettingView : MonoBehaviour
{
    public Button aboutBtn;
    public Button reportBtn;
    public Button updateBtn;
    public Button paliVoiceTypeBtn;
    public Text paliVoiceTypeText;
    public Button paliVoiceGenderBtn;
    public Text paliVoiceGenderText;
    public Button articleClassifyBtn;
    public Text versionText;
    public GameObject updateRedPoint;
    public GameObject reportGO;
    public Button transContentSliderBtn;
    public Slider transContentSliderToggle;
    public CommonGroupView commonGroupView;
    // Start is called before the first frame update
    void Start()
    {
        //if (GameManager.Instance().canUpdate)
        //    SetUpdateRedPoint();
        paliVoiceTypeText.text = SettingManager.Instance().GetPaliVoiceTypeName();
        paliVoiceGenderText.text = SettingManager.Instance().GetPaliVoiceGenderName();
        transContentSliderToggle.value = SettingManager.Instance().GetTransContent();
        transContentSliderToggle.onValueChanged.AddListener(OnTransContentToggleValueChanged);
        transContentSliderBtn.onClick.AddListener(OnTransContentBtnClick);
        aboutBtn.onClick.AddListener(OnAboutBtnClick);
        updateBtn.onClick.AddListener(OnUpdateBtnClick);
        reportBtn.onClick.AddListener(OnReportBtnClick);
        paliVoiceTypeBtn.onClick.AddListener(OnPaliVoiceTypeBtnClick);
        paliVoiceGenderBtn.onClick.AddListener(OnPaliVoiceGenderBtnClick);
        articleClassifyBtn.onClick.AddListener(OnArticleClassifyBtnClick);
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
    void OnReportBtnClick()
    {
        reportGO.SetActive(true);
    }
    void OnArticleClassifyBtnClick()
    {
        //commonGroupView.InitSettingOptions(new List<string> { "默认", "CSCD4" }, 0, (a) => {SettingManager.Instance().SetTransContent  return null; });
        //commonGroupView.gameObject.SetActive(true);
    }
    void OnPaliVoiceGenderBtnClick()
    {
        int sID = (int)SettingManager.Instance().GetPaliVoiceGender();
        commonGroupView.InitSettingOptions(new List<string> { "男声", "女声" }, sID, (id) => {
            SettingManager.Instance().SetPaliVoiceGender((PaliSpeakVoiceGender)id); 
            paliVoiceGenderText.text = SettingManager.Instance().GetPaliVoiceGenderName(); 
            return null; });
        commonGroupView.gameObject.SetActive(true);
    }
    void OnPaliVoiceTypeBtnClick()
    {
        int sID = (int)SettingManager.Instance().GetPaliVoiceType();
        commonGroupView.InitSettingOptions(new List<string> { "印度风格", "缅甸风格" }, sID, (id) => { 
            SettingManager.Instance().SetPaliVoiceType((PaliSpeakVoiceType)id);
            paliVoiceTypeText.text = SettingManager.Instance().GetPaliVoiceTypeName();
            return null; });
        commonGroupView.gameObject.SetActive(true);
    }
    void OnAboutBtnClick()
    {
        commonGroupView.InitAboutView();
        commonGroupView.gameObject.SetActive(true);
    }
    void OnUpdateBtnClick()
    {
        UpdateManager.Instance().CheckUpdateOpenPage(this);

        //StartCoroutine(UpdateManager.Instance().Test());
    }
    public void SetUpdatePage(UpdateInfo currentUInfo)
    {
        commonGroupView.InitUpdateView(currentUInfo);
        commonGroupView.gameObject.SetActive(true);
    }
    public void SetUpdateRedPoint()
    {
        updateRedPoint.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
