using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemArticleSentenceView : MonoBehaviour
{
    public Text sentenceText;
    public Toggle selectToggle;
    public RectTransform backRT;
    public string sentenceContent;
    //bool isInit = false;
    public void Init(string sentence)
    {
        //Debug.LogError(this.isActiveAndEnabled);
        //Debug.LogError(this.gameObject.activeSelf);
        sentenceText.text = sentence;
        sentenceContent = sentence;
    }
    IEnumerator SetHeight()
    {
        yield return null;

        float height = sentenceText.GetComponent<RectTransform>().sizeDelta.y + 50;

        backRT.sizeDelta = new Vector2(backRT.sizeDelta.x, height);// = new Vector2(detailScrollContent.sizeDelta.x, height);
        sentenceText.rectTransform.localPosition = new Vector3(sentenceText.rectTransform.localPosition.x, 0, sentenceText.rectTransform.localPosition.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetHeight());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Debug.LogError(this.isActiveAndEnabled);
        //    Debug.LogError(this.gameObject.activeSelf);
        //}
    }
}
