using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UpdateManager;

public class SettingView : MonoBehaviour
{
    public Button aboutBtn;
    public Button reportBtn;
    public Button updateBtn;
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

        transContentSliderToggle.value = SettingManager.Instance().GetTransContent();
        transContentSliderToggle.onValueChanged.AddListener(OnTransContentToggleValueChanged);
        transContentSliderBtn.onClick.AddListener(OnTransContentBtnClick);
        aboutBtn.onClick.AddListener(OnAboutBtnClick);
        updateBtn.onClick.AddListener(OnUpdateBtnClick);
        reportBtn.onClick.AddListener(OnReportBtnClick);
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
