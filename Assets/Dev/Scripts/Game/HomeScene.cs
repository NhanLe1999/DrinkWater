using FlutterUnityIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public string sttate = "";
    public static HomeScene Instance { get; private set; }

    [SerializeField] PageViewController pageViewController;
    [SerializeField] DataCupGame dataCupGame = null;
    [SerializeField] GameObject objPlay = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = null;
        }
        Instance = this;
    }

    void Start()
    {
        ScStaticScene.dataCupGame = dataCupGame;
        pageViewController.SetData(dataCupGame.dataCups);
    }

    public void OnPlayGame()
    {
        if (sttate.Equals("play"))
        {
            ScStaticScene.NumCup = 1;
            ScStaticScene.State = 1;
        }
        else if (sttate.Equals("drink"))
        {
            ScStaticScene.NumCup = 1;
            ScStaticScene.State = 2;
        }
        else if (sttate.Equals("buffet"))
        {
            ScStaticScene.State = 3;
            objPlay.SetActive(false);
            ScStaticScene.NumCup = 2;
            ScStaticScene.dataCup = dataCupGame.dataCups[UnityEngine.Random.Range(0, dataCupGame.dataCups.Count-1)];
            HelperManager.OnLoadScene(ScStaticScene.GAME_SCENE);
        }
    }    

    public void OnBack()
    {
        UnityMessageManager.Instance.SendMessageToFlutter("back");
    }
}
