using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WDColorPickerProgress : MonoBehaviour
{
    public Slider progressSlider;  
    public Image colorDisplay;    

    void Start()
    {
        progressSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        Color newColor = Color.Lerp(Color.red, Color.blue, value); 
        colorDisplay.color = newColor;
    }
}
