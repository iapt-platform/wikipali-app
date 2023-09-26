using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticleTitleReturnBtn : MonoBehaviour
{
    public Image img;
    public Button btn;
    public Text title;
    public ArticleView articleView;
    // Start is called before the first frame update
    void Start()
    {
        Init();

        btn.onClick.AddListener(OnBtnClick);

    }
    public void Init()
    {
        img.gameObject.SetActive(false);
        title.text = "圣典";
    }
    public void SetPath(string path)
    {
        img.gameObject.SetActive(true);
        title.text = path;
    }
    public void OnBtnClick()
    {
        SpeechManager.Instance().StopLoadSpeech();
        articleView.ReturnBtnClick();
    }
    // Update is called once per frame
    //void Update()
    //{

    //}
}
