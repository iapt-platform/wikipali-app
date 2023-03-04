using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicGroupView : MonoBehaviour
{
    public Button returnBtn;

    //DicGroupPopView
    public DicGroupPopView dicGroupPopView;
    public ItemDicGroupEditView editItem;

    // Start is called before the first frame update
    void Start()
    {
        returnBtn.onClick.AddListener(OnCloseBtnClick);

    }
    public void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
