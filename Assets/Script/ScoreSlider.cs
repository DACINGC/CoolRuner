using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSlider : MonoBehaviour
{
    private Slider mySlider;
    public Text percentage;
    public int maxValue = 1000;
    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
        mySlider.maxValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        percentage.text = (mySlider.value / maxValue * 100).ToString() + "%";
    }
}
