using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemArticleSentenceView : MonoBehaviour
{
    public Text sentenceText;
    public Toggle selectToggle;
    public void Init(string sentence)
    {
        sentenceText.text = sentence;
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
