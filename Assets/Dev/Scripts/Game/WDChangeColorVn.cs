using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDChangeColorVn : MonoBehaviour
{
    [SerializeField] public SliderColor sli1 = null;
    [SerializeField] public SliderColor sli2 = null;

    public void OnChangeColor1()
    {

    }   

    public void OnChangeColor2()
    {

    }   

    void Start()
    {
        
    }

    public void OnOkeClick()
    {
        gameObject.SetActive(false);
    }

    public void SetActiviveSlide(int numColor)
    {
        if(numColor == 1)
        {
            sli2.gameObject.SetActive(false);
        }
    }
}
