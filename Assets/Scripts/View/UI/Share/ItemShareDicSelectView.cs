using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShareDicSelectView : MonoBehaviour
{
    public Toggle toggle;
    public Text title;
    public string dicTableID;
    public string dicTableName;

    public void Init(string _dicTableName, string _dicTitle)
    {
        title.text = _dicTitle;
        dicTableName = _dicTableName;
        dicTableID = _dicTitle;
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
