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
    public Button paliVoiceSpeedBtn;
    public Text paliVoiceSpeedText;
    public Button articleClassifyBtn;
    public Text versionText;
    public GameObject updateRedPoint;
    public GameObject reportGO;
    public Button transContentSliderBtn;
    public Slider transContentSliderToggle;
    public Button openLastSliderBtn;
    public Slider openLastSliderToggle;
    public CommonGroupView commonGroupView;
    // Start is called before the first frame update
    void Start()
    {
        //if (GameManager.Instance().canUpdate)
        //    SetUpdateRedPoint();
        paliVoiceSpeedText.text = SettingManager.Instance().GetPaliVoiceSpeedName();
        paliVoiceTypeText.text = SettingManager.Instance().GetPaliVoiceTypeName();
        paliVoiceGenderText.text = SettingManager.Instance().GetPaliVoiceGenderName();
        transContentSliderToggle.value = SettingManager.Instance().GetTransContent();
        transContentSliderToggle.onValueChanged.AddListener(OnTransContentToggleValueChanged);
        transContentSliderBtn.onClick.AddListener(OnTransContentBtnClick);
        openLastSliderToggle.value = SettingManager.Instance().GetOpenLast();
        openLastSliderToggle.onValueChanged.AddListener(OnOpenLastToggleValueChanged);
        openLastSliderBtn.onClick.AddListener(OnOpenLastBtnClick);
        aboutBtn.onClick.AddListener(OnAboutBtnClick);
        updateBtn.onClick.AddListener(OnUpdateBtnClick);
        reportBtn.onClick.AddListener(OnReportBtnClick);
        paliVoiceSpeedBtn.onClick.AddListener(OnPaliVoiceSpeedBtnClick);
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
    void OnOpenLastToggleValueChanged(float value)
    {
        //Debug.LogError(value);
        SettingManager.Instance().SetOpenLast((int)value);
    }
    void OnOpenLastBtnClick()
    {
        if (openLastSliderToggle.value > 0.5f)
        {
            openLastSliderToggle.value = 0;
        }
        else
        {
            openLastSliderToggle.value = 1;
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
        commonGroupView.InitSettingOptions("巴利朗读声音", new List<string> { "男声", "女声" }, sID, (id) =>
        {
            SettingManager.Instance().SetPaliVoiceGender((PaliSpeakVoiceGender)id);
            paliVoiceGenderText.text = SettingManager.Instance().GetPaliVoiceGenderName();
            return null;
        });
        commonGroupView.gameObject.SetActive(true);
    }
    void OnPaliVoiceTypeBtnClick()
    {
        int sID = (int)SettingManager.Instance().GetPaliVoiceType();
        commonGroupView.InitSettingOptions("巴利朗读风格", new List<string> { "印度风格", "缅甸风格" }, sID, (id) =>
        {
            SettingManager.Instance().SetPaliVoiceType((PaliSpeakVoiceType)id);
            paliVoiceTypeText.text = SettingManager.Instance().GetPaliVoiceTypeName();
            return null;
        });
        commonGroupView.gameObject.SetActive(true);
    }
    void OnPaliVoiceSpeedBtnClick()
    {
        int sID = SettingManager.PaliSpeakVoiceSpeedList.IndexOf((int)SettingManager.Instance().GetPaliVoiceSpeed());
        commonGroupView.InitSettingOptions("巴利朗读语速", new List<string> { "0", "-10%", "-20%", "-30%", "-40%" }, sID, (id) =>
        {
            SettingManager.Instance().SetPaliVoiceSpeed((PaliSpeakVoiceSpeed)SettingManager.PaliSpeakVoiceSpeedList[(int)id]);
            paliVoiceSpeedText.text = SettingManager.Instance().GetPaliVoiceSpeedName();
            return null;
        });
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
