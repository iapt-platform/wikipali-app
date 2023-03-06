using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGroupDictView : MonoBehaviour
{
    public Toggle starToggle;
    public Button shareBtn;
    public PopView popView;
    // Start is called before the first frame update
    void Start()
    {
        starToggle.onValueChanged.AddListener(OnToggleValueChanged);

    }
    void OnToggleValueChanged(bool value)
    {
        popView.RefreshGroupList();
        popView.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
