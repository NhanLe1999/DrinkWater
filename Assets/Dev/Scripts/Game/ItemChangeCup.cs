using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChangeCup : MonoBehaviour
{
    [SerializeField] Image imgIc = null;
    [SerializeField] GameObject objAds = null;

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
        if(!dataCup.IsUseAds)
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
                var cpns = GameObject.FindObjectsOfType<MetaballParticleClass>();

                foreach (var c in cpns)
                {
                    c.Active = false;
                }

                var cpns1 = GameObject.FindObjectsOfType<TopingItem>();
                foreach (var c in cpns1)
                {
                    Destroy(c.gameObject);
                }


                ScStaticScene.dataCup = dataCup;
                LogicGame.Instance.LoadUi();
            }
        };

        if(objAds.activeSelf)
        {
            AdsFlutterAndUnityManager.instance.OnShowAdsInter(callback, ScStaticScene.NAME_TYPE_ADS_ITEM);
        }
        else
        {
            callback?.Invoke(true);
        }

       
    }    
}
