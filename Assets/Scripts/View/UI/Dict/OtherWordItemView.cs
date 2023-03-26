using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherWordItemView : MonoBehaviour
{
    public Button titleBtn;
    public Image dropDownImg;
    public Text titleTxt;
    public GameObject wordItemGo;
    List<string> caseWordList;
    //public Text wordBtn;
    public void Init(List<string> _caseWordList)
    {
        caseWordList = _caseWordList;
        if (caseWordList.Count == 0)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
    public void SetLayerEnd()
    {
        this.GetComponent<RectTransform>().SetAsLastSibling();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
