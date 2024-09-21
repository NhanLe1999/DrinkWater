using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderColor : MonoBehaviour
{
    public Slider slider;
    public bool IsNgang = false;
    [SerializeField] RectTransform _rectTransform;

    public Action<Color> callback = null;


    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        this.StartCoroutine(ChangPoint());
    }

    public void OnSliderValueChanged(float value)
    {
        var color = Color.HSVToRGB(value, 1.0f, 1.0f);

        callback?.Invoke(color);
    }

    IEnumerator ChangPoint()
    {
        yield return new WaitForSeconds(0.1f);

        if(IsNgang)
        {
            _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, -32.6f);
            _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, -29.6f);
        }
        else
        {
            _rectTransform.offsetMin = new Vector2(-53, _rectTransform.offsetMin.y);
            _rectTransform.offsetMax = new Vector2(-17, _rectTransform.offsetMax.y);
        }
      
    }

    public void UpdateStateHandleRect(float handel)
    {
        slider.value = handel;
    }    

}
