using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectCup : MonoBehaviour
{
    public DataCup data = null;
    [SerializeField] Image imgIcCup = null;

    void Start()
    {
        
    }

   public void SetDataCup(DataCup da)
    {
        data = da;
    }

    public void UpdateUI()
    {
        imgIcCup.sprite = data.sprImgSelect;
        imgIcCup.SetNativeSize();
    }

    public void OnClickCup()
    {
        Action<bool> callback = isSucess => { 
            if(isSucess)
            {
                ScStaticScene.dataCup = data;
                HelperManager.LoadScene(ScStaticScene.GAME_SCENE);
            }
        };


#if UNITY_EDITOR
        callback?.Invoke(true);
        return;
#endif

        AdsFlutterAndUnityManager.instance.OnShowAdsInter(callback, ScStaticScene.NAME_TYPE_ADS_ITEM);

    }
}
