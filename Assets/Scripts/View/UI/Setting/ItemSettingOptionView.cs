using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSettingOptionView : MonoBehaviour
{
    public GameObject toggleItem;
    public RectTransform toggleItemRT;
    List<string> nameList;
    int selection;
    Func<object, object> SettingOptionFin;

    public void Init(List<string> _nameList, int _selection, Func<object, object> fin)
    {
        SettingOptionFin = fin;
        nameList = _nameList;
        selection = _selection;
        RefreshToggleList();
        this.gameObject.SetActive(true);
    }
    List<Toggle> itemList = new List<Toggle>();
    void DestroyToggleList()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();
    }
    void RefreshToggleList()
    {
        DestroyToggleList();
        float height = toggleItemRT.sizeDelta.y;
        int gl = nameList.Count;
        for (int i = 0; i < gl; i++)
        {
            GameObject inst = Instantiate(toggleItem.gameObject, toggleItem.transform.parent, false);
            inst.transform.position = toggleItem.transform.position;
            inst.GetComponent<RectTransform>().localPosition -= i * Vector3.up * height;
            Toggle toggle = inst.GetComponent<Toggle>();
            if (i == selection)
                toggle.isOn = true;

            Transform label = inst.transform.Find("ToggleLabel");
            label.GetComponent<Text>().text = nameList[i];

            inst.SetActive(true);
            itemList.Add(toggle);
        }
    }
    private void OnDisable()
    {
        int l = itemList.Count;
        for (int i = 0; i < l; i++)
        {
            if (itemList[i].isOn)
                SettingOptionFin(i);
        }
        //Debug.LogError("!!!!!!!!!!!!!!!!!!");
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {

    }
}
