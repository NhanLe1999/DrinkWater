using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopingSelectItem : MonoBehaviour
{
    [SerializeField] Image imgIc = null;

    DataToping dataToping = null;

    public void SetData(DataToping da)
    {
        dataToping = da;
    }

    public void UpdateUi()
    {
        imgIc.sprite = dataToping.spr;
        imgIc.SetNativeSize();

        var rect = imgIc.GetComponent<RectTransform>();
        var size = rect.sizeDelta;
        float scaleX = 126 / size.x;
        float scaleY = 90 / size.y;

        float scale = Math.Min(scaleX, scaleY);
        rect.localScale = Vector3.one * scale;
    }

    public void OnChangeCup()
    {
        LogicGame.Instance.LoadToping(dataToping.spr);
    }
}
