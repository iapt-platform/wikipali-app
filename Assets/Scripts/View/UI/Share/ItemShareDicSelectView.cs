using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ItemShareDicSelectView : MonoBehaviour
{
    public Toggle toggle;
    public Text title;
    public string dicTableID;
    public string dicTableName;

    public void Init(MatchedWordDetail word)
    {
        title.text = word.dicName;
        dicTableID = word.dicName;
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
