using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChangeCup : MonoBehaviour
{
    [SerializeField] Image imgIc = null;

    DataCup dataCup = null;
    void Start()
    {
        
    }

   public void SetData(DataCup da)
    {
        dataCup = da;
    }

    public void UpdateUi()
    {
        imgIc.sprite = dataCup.sprImgChange;
        imgIc.SetNativeSize();
    }
}
