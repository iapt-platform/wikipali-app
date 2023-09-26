using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitView : MonoBehaviour
{
    public Slider progressSlider;
    public Text progressText;
    public Text titleText;

    public void Init(string title)
    {
        titleText.text = title;
        progressSlider.value = 0;
        progressText.text =  "0%";
    }
    public void SetProgess(float progress)
    {
        progressSlider.value = progress;
        progressText.text = (int)(progress*100) + "%";
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
