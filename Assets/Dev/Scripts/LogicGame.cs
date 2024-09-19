using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Water2D;

public enum StateGame
{
    Game,
    Pause
}

public class LogicGame : SingletonMono<LogicGame>
{
    public Transform TrsObj = null;
    public Transform TrsObjUp = null;

    public Transform pointCup = null;

    public DataCupGame dataCupGame = null;

    public float baseGravity = -9.81f; 
    [SerializeField] Transform parentCup = null;
    private DringCupManager currentDrinkCupManager = null;

    bool isDown = false;

    private void Start()
    {
        LoadUi();
    }

    public void LoadUi()
    {
        if(ScStaticScene.dataCup == null)
        {
            return;
        }
        if(currentDrinkCupManager != null)
        {
            Destroy(currentDrinkCupManager.gameObject);
        }

        var cup = Instantiate(ScStaticScene.dataCup.prefabCup, parentCup);
        cup.transform.position = pointCup.position;
    }

    private void Update()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Vector3 acceleration = Input.acceleration;
            Physics2D.gravity = new Vector2(acceleration.x * Mathf.Abs(baseGravity), acceleration.y * Mathf.Abs(baseGravity));
        }
    }
}
