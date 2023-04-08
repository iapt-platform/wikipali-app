using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSpace : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        //text.text = "New T各个 \tTextNew Text\r\nNew Text\t\t Text\r\n各 TextNew TextNew Text\r\nNew TextNew Text";
        //text.text = "|语种|缩写|全称|\r\n|-|-|-|\r\n|巴利|kri|kriyā|\r\n|汉|**动**|动词|\r\n|英|**v.**|verb|";
        text.text = "|语种|缩写|全称|\r\n|-|-|-|\r\n|巴利|kri|kriyā|\r\n|汉|<b>动</b>|动词|\r\n|英|**v.**|verb|";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
