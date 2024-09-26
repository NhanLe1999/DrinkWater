using FlutterUnityIntegration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataShowAds
{
    public Action<bool> callbackAds;
}

public class AdsFlutterAndUnityManager : MonoBehaviour
{
    public static AdsFlutterAndUnityManager instance = null;

    public Action<bool> callbackAds;

    void Start()
    {
        if (AdsFlutterAndUnityManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    //showAds_inter_default, showAds_inter_reciver_item

    public void OnShowAdsInter(Action<bool> callback, string type = "default")
    {
        Audio.Pause();
        Audio.PauseBackgroundMusic();
        Time.timeScale = 0.0f;
        callbackAds = callback;
        UnityMessageManager.Instance.SendMessageToFlutter("showAds_inter_" + type);
    }

    public void OnReciverCallbackAdsInter(string message)
    {
        Time.timeScale = 1.0f;
        Audio.Resume();
        Audio.ResumeBackgroundMusic();
        callbackAds?.Invoke(message.Equals("showAds_reciver_item_true"));
        callbackAds = null;
    }

    public void SenNameSceneToFluter(string nameScene)
    {
        UnityMessageManager.Instance.SendMessageToFlutter(nameScene);
        Debug.Log(nameScene);
    }
}
