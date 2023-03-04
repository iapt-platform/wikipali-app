using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDicGroupEditView : MonoBehaviour
{
    public GameObject dicGroupPopView;
    public Text titleText;
    public Button editBtn;
    // Start is called before the first frame update
    void Start()
    {
        editBtn.onClick.AddListener(OnEditBtnClick);

    }

    public void OnEditBtnClick()
    {
        dicGroupPopView.gameObject.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
