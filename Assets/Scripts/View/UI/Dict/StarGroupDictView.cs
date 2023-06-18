using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGroupDictView : MonoBehaviour
{
    public Toggle starToggle;
    public Button shareBtn;
    public Button voiceBtn;
    public AudioSource voiceSource;
    public PopView popView;
    public ShareView shareView;
    // Start is called before the first frame update
    void Awake()
    {
        starToggle.onValueChanged.AddListener(OnToggleValueChanged);
        shareBtn.onClick.AddListener(OnShareBtnClick);
        voiceBtn.onClick.AddListener(OnVoiceBtnClick);

    }
    string currVoiceWord;
    public void OnVoiceBtnClick()
    {
        string readWord = SpeechGeneration.Instance().ReplaceWordTGL(DictManager.Instance().currWord);
        if (voiceSource.clip != null&& currVoiceWord == readWord)
        {
            voiceSource.Play();
            return;
        }
        AudioClip ac =  SpeechGeneration.Instance().SpeekSI(readWord,-10);
        if (ac != null)
        {
            currVoiceWord = readWord;
            voiceSource.clip = ac;
            voiceSource.Play();
        }
    }
    //public void OnVoiceBtnClick()
    //{
    //    string readWord = SpeechGeneration.Instance().ReplaceWord(DictManager.Instance().currWord);
    //    if (voiceSource.clip != null && currVoiceWord == readWord)
    //    {
    //        voiceSource.Play();
    //        return;
    //    }
    //    AudioClip ac = SpeechGeneration.Instance().Speek(readWord);
    //    if (ac != null)
    //    {
    //        currVoiceWord = readWord;
    //        voiceSource.clip = ac;
    //        voiceSource.Play();
    //    }
    //}
    public void OnShareBtnClick()
    {
        shareView.gameObject.SetActive(true);
        shareView.Init();
    }
    bool isSet = false;
    public void SetToggleValue(bool isOn)
    {
        //bug:??????????第一次进收藏过的单词，点击星后，没有触发OnToggleValueChanged
        //已解决：注册事件改为Awake就好了，事件还未注册就设置toggle了
        if (starToggle.isOn != isOn)
        {
            isSet = true;
            starToggle.isOn = isOn;
        }
    }
    void OnToggleValueChanged(bool value)
    {
        if (isSet)
        {
            isSet = false;
            return;
        }
        popView.Init(PopViewType.SaveDic);
        popView.RefreshGroupList();
        popView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
