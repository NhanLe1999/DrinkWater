using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopingSelectItem : MonoBehaviour
{
    [SerializeField] Image imgIc = null;
    DataToping dataToping = null;
    [SerializeField] GameObject objAds = null;

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


    public void EnableBtnAds(bool isEnable)
    {
        if(!isEnable)
        {
            objAds.SetActive(false);
        }
#if UNITY_EDITOR
        objAds.SetActive(false);
#endif
    }



    public void OnChangeCup()
    {
        Action<bool> callback = isSucess => { 
            if(isSucess)
            {
                objAds.SetActive(false);
                LogicGame.Instance.LoadToping(dataToping);
            }
        };

        if (objAds.activeSelf)
        {
            AdsFlutterAndUnityManager.instance.OnShowAdsInter(callback, ScStaticScene.NAME_TYPE_ADS_ITEM);
        }
        else
        {
            callback?.Invoke(true);
        }

    }
}
