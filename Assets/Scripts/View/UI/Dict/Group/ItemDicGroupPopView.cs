using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DictManager;

public class ItemDicGroupPopView : MonoBehaviour
{
    public Toggle selectToggle;
    public Text wordCountText;
    public Text wordGroupName;
    public DicGroupInfo dicGroupInfo;

    public void Init(DicGroupInfo _dicInfo)
    {
        dicGroupInfo = _dicInfo;
        if (dicGroupInfo.wordList != null)
            wordCountText.text = dicGroupInfo.wordList.Count.ToString();
        wordGroupName.text = dicGroupInfo.groupName;

    }
    public bool GetSelectState()
    {
        return selectToggle.isOn;
    }
    public void SetToggle()
    { 
    
    
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
