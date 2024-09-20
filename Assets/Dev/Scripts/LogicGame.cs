using Cysharp.Threading.Tasks;
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
    [SerializeField] GameObject prefabWater = null;
    [SerializeField] GameObject prefabToping = null;
    [SerializeField] public DataTopingGame dataTopingGame = null;

    [SerializeField] GameObject objChangeColorNc = null;

    [SerializeField] GameObject objFruit = null;
    [SerializeField] GameObject objChangeCup = null;

    int numWater = 0;

    public Vector2 SizeCamera = Vector2.one;

    List<Water2D_Spawner> water2D_Spawners = new();

    bool isDown = false;

    private void Start()
    {
        SizeCamera = HelperManager.GetSizeCamera();

        LoadNumVn(1);



        LoadUi();
    }

    public void LoadNumVn(int num)
    {
        numWater = num;

        SizeCamera = HelperManager.GetSizeCamera();
        float disX = 0.5f;

        var sizee = prefabWater.GetComponent<SpriteRenderer>().bounds.size;
        var SumSizeX = sizee.x * num + disX * (num - 1);

        var xBegin = -SumSizeX / 2;

        for (int i = 0; i < num; i++)
        {
            var objWater = Instantiate(prefabWater, null);

            water2D_Spawners.Add(objWater.GetComponent<Water2D_Spawner>());

            var size = objWater.GetComponent<SpriteRenderer>().bounds.size;
            xBegin += size.x / 2;
            objWater.transform.position = new Vector3(xBegin, SizeCamera.y / 2, 0);
            xBegin += size.x / 2 + disX;
        }
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
        currentDrinkCupManager = cup.GetComponent<DringCupManager>();
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

    public void LoadToping(Sprite spr)
    {
        var toping = Instantiate(prefabToping, null);
        var cpn = toping.GetComponent<TopingItem>();
        var size = cpn.sprImg.bounds.size;
        toping.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180, 180));
        toping.transform.position = new Vector3(UnityEngine.Random.Range(DringCupManager.Instance.p1.position.x + size.x / 2, DringCupManager.Instance.p2.position.x - size.x / 2), SizeCamera.y / 2 + size.y / 2);
        cpn.SetData(spr);
        cpn.OnUpdateUi();
        toping.transform.localScale = Vector3.one * 0.5f;
    }

    public void OnChangeColorForVnClick()
    {
        objChangeColorNc.gameObject.SetActive(true);

        var cpn = objChangeColorNc.GetComponent<WDChangeColorVn>();

        cpn.SetActiviveSlide(numWater);
        cpn.sli1.callback = color => {
            water2D_Spawners[0].OnUpdateColor(color);
        };

        if (water2D_Spawners.Count > 1)
        {
            cpn.sli2.callback = color => {
                water2D_Spawners[1].OnUpdateColor(color);
            };
        }    
    }

    public void OnBack()
    {
        HelperManager.OnLoadScene(ScStaticScene.HOME_SCENE);
    }   
    
    public void OnShowFruit()
    {
        objFruit.SetActive(true);
    }   
    
    public void OnShowChangeCup()
    {
        objChangeCup.SetActive(true);
    }
}
