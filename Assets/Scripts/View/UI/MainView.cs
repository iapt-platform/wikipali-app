//主界面
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public Toggle dicToggle;
    public Toggle articleToggle;
    public Toggle userToggle;
    public Toggle settingToggle;
    public DicView dicView;
    public ArticleView articleView;
    public UserView userView;
    public SettingView settingView;
    // Start is called before the first frame update
    void Start()
    {
        dicToggle.onValueChanged.AddListener(OnDicToggleValueChanged);
        articleToggle.onValueChanged.AddListener(OnArticleToggleValueChanged);
        userToggle.onValueChanged.AddListener(OnUserToggleValueChanged);
        settingToggle.onValueChanged.AddListener(OnSettingToggleValueChanged);
    }
    void OnDicToggleValueChanged(bool value)
    {
        dicView.gameObject.SetActive(value);

    }
    void OnArticleToggleValueChanged(bool value)
    {
        articleView.gameObject.SetActive(value);


    }
    void OnUserToggleValueChanged(bool value)
    {
        userView.gameObject.SetActive(value);


    }
    void OnSettingToggleValueChanged(bool value)
    {
        settingView.gameObject.SetActive(value);


    }
    // Update is called once per frame
    void Update()
    {

    }
}
