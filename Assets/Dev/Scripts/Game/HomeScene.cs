using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : MonoBehaviour
{
    public static HomeScene Instance { get; private set; }

    [SerializeField] PageViewController pageViewController;
    [SerializeField] DataCupGame dataCupGame = null;

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
}
