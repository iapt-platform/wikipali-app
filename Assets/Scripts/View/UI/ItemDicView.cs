using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDicView : MonoBehaviour
{
    public Button btn;
    public Text nameTxt;
    public Text detailTxt;
    public void SetMeaning(string name,string detail)
    {
        nameTxt.text = name;
        detailTxt.text = detail;
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
