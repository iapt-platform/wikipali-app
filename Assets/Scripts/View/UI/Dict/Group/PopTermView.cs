using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//术语弹窗
public class PopTermView : MonoBehaviour
{
    public Text titleText;
    public RectTransform detailRect;
    public Text detailText;
    public Button closeBackGroupBtn;
    public void Init(string title, string detail)
    {
        titleText.text = MarkdownText.PreprocessText(title, titleText.fontSize);
        detailText.text = MarkdownText.PreprocessText(detail, detailText.fontSize);
        this.gameObject.SetActive(true);
        //content size fitter 下一帧才会刷新富文本格式后的text大小
        //等下一帧UI刷新后获取位置
        StartCoroutine(SetHeight());
        //SetHeight();
    }
    IEnumerator SetHeight()
    {
        yield return null;
        float height = detailText.rectTransform.sizeDelta.y;
        detailRect.sizeDelta = new Vector2(detailRect.sizeDelta.x, height);

    }

    //void SetHeight()
    //{
    //    float height = detailText.rectTransform.sizeDelta.y;
    //    detailRect.sizeDelta = new Vector2(detailRect.sizeDelta.x, height);
    //    //detailText.rectTransform.localPosition = new Vector3(detailText.rectTransform.localPosition.x, 30, detailText.rectTransform.localPosition.z);
    //}
    // Start is called before the first frame update
    void Start()
    {
        closeBackGroupBtn.onClick.AddListener(OnCloseBackBtnClick);

    }
    public void OnCloseBackBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
