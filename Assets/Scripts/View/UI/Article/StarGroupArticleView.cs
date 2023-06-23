using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGroupArticleView : MonoBehaviour
{
    public Toggle starToggle;
    public Button shareBtn;
    public Button voiceBtn;
    public AudioSource voiceSource;
    public PopArticleSentenceSelectView popSentenceSelectView;
    public PopView popView;
    //public ShareView shareView;
    // Start is called before the first frame update
    void Awake()
    {
        starToggle.onValueChanged.AddListener(OnToggleValueChanged);
        shareBtn.onClick.AddListener(OnShareBtnClick);
        voiceBtn.onClick.AddListener(OnVoiceBtnClick);

    }
    string currVoiceArticle;
    //public void OnVoiceBtnClick()
    //{
    //    string readWord = SpeechGeneration.Instance().ReplaceWord(ArticleController.Instance().testPl);
    //    if (voiceSource.clip != null && currVoiceArticle == readWord)
    //    {
    //        voiceSource.Play();
    //        return;
    //    }
    //    AudioClip ac = SpeechGeneration.Instance().Speek(readWord);
    //    if (ac != null)
    //    {
    //        currVoiceArticle = readWord;
    //        voiceSource.clip = ac;
    //        voiceSource.Play();
    //    }
    //}
    public void OnVoiceBtnClick()
    {
        string readArticle = SpeechGeneration.Instance().ReplaceWord(ArticleController.Instance().testPl);
        //string test = "evaṃ me sutaṃ– ekaṃ samayaṃ bhagavā ";
        //string readArticle = SpeechGeneration.Instance().ReplaceWordTGL(test);
        if (voiceSource.clip != null && currVoiceArticle == readArticle)
        {
            voiceSource.Play();
            return;
        }
        AudioClip ac = SpeechGeneration.Instance().SpeekSI(readArticle, -40);
        if (ac != null)
        {
            currVoiceArticle = readArticle;
            voiceSource.clip = ac;
            voiceSource.Play();
        }
    }
    //public void OnVoiceBtnClick()
    //{
    //    string readArticle = ArticleController.Instance().testCN;
    //    if (voiceSource.clip != null && currVoiceArticle == readArticle)
    //    {
    //        voiceSource.Play();
    //        return;
    //    }
    //    AudioClip ac = SpeechGeneration.Instance().SpeekCN(readArticle);
    //    if (ac != null)
    //    {
    //        currVoiceArticle = readArticle;
    //        voiceSource.clip = ac;
    //        voiceSource.Play();
    //    }
    //}
    public void OnShareBtnClick()
    {
        popSentenceSelectView.Init();
        popSentenceSelectView.gameObject.SetActive(true);

        //shareView.gameObject.SetActive(true);
        //shareView.Init();
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
        popView.Init(PopViewType.SaveArticle);
        popView.RefreshGroupList();
        popView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
