using DG.Tweening;
using FlutterUnityIntegration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
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
    [SerializeField] GameObject prefabTopingDa = null;
    [SerializeField] public DataTopingGame dataTopingGame = null;

    [SerializeField] GameObject objChangeColorNc = null;

    [SerializeField] RectTransform objFruit = null;
    [SerializeField] RectTransform objChangeCup = null;
    [SerializeField] SliderColor sliderColor = null;
    [SerializeField] Transform trsPointTop = null;
    [SerializeField] SelectTopingManager selectTopingManager = null;

    public List<DataToping> dataToping = new();

    bool isAutoToping = false;


    public Vector2 pointcheckCup = Vector2.one;

    int numWater = 0;

    bool isEnableFruit = false;
    bool isEnableChangCup = false;

    bool isRunAnimFruit = false;
    bool isRunAnimChangeCup = false;


    public Vector2 SizeCamera = Vector2.one;

    List<Water2D_Spawner> water2D_Spawners = new();

    bool isDown = false;

    private void Start()
    {
        UnityMessageManager.Instance.SendMessageToFlutter("sound_false");
        AdsFlutterAndUnityManager.instance.SenNameSceneToFluter(ScStaticScene.GAME_SCENE);

        Audio.PlayBackgroundMusic(ScStaticScene.SFX_Music_Game);

        SizeCamera = HelperManager.GetSizeCamera();
        sliderColor.callback = co => {
            DringCupManager.Instance?.SetColor(co);
        };

        if(ScStaticScene.NumCup == 0)
        {
            ScStaticScene.NumCup = 1;
        }    

        this.StartCoroutine(LoadNumVn(ScStaticScene.NumCup));
    }

    public IEnumerator LoadNumVn(int num)
    {
        yield return new WaitForSeconds(0.1f);
        numWater = num;

        SizeCamera = HelperManager.GetSizeCamera();
        float disX = 0.01f;

        var sizee = prefabWater.GetComponent<SpriteRenderer>().bounds.size;
        var SumSizeX = sizee.x * num + disX * (num - 1);

        var xBegin = -SumSizeX / 2;

        for (int i = 0; i < num; i++)
        {
            var objWater = Instantiate(prefabWater, null);

            water2D_Spawners.Add(objWater.GetComponent<Water2D_Spawner>());

            var size = objWater.GetComponent<SpriteRenderer>().bounds.size;
            xBegin += size.x / 2;
            objWater.transform.position = new Vector3(xBegin, trsPointTop.position.y, 0);
            xBegin += size.x / 2 + disX;
        }

        pointcheckCup = new Vector2(0, trsPointTop.position.y - sizee.y);
        LoadUi();
        if (ScStaticScene.State == 3 || ScStaticScene.State == 2)
        {
            isAutoToping = true;
            this.StartCoroutine(CheckTimeLoadToping());
            this.StartCoroutine(onAutoLoadToping());
        }

    }

    public void ResetWater()
    {
        foreach(var it in water2D_Spawners)
        {
           for(int i = 0; i < it.WaterDropsObjects.Length; i++)
            {
                it.WaterDropsObjects[i] = null;
            }    
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
        currentDrinkCupManager.Init();
        currentDrinkCupManager.UpdateScale();
        currentDrinkCupManager.IdCup = ScStaticScene.dataCup.IdCup;

        dataToping = dataTopingGame.GetDataTopingByCupId(ScStaticScene.dataCup.IdCup);
        selectTopingManager.OnLoadData();
    }

    private void Update()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Vector3 acceleration = Input.acceleration;
            Physics2D.gravity = new Vector2(acceleration.x * Mathf.Abs(baseGravity), acceleration.y * Mathf.Abs(baseGravity));

            float tiltX = Input.acceleration.x;

            float angle = Mathf.Asin(tiltX) * Mathf.Rad2Deg;

            if(angle <= -45 || angle >= 45)
            {
                var cpns = GameObject.FindObjectsOfType<MetaballParticleClass>();

                foreach (var c in cpns)
                {
                    if (c.Active)
                    {
                        Audio.Play(ScStaticScene.SFX_sound_un);
                        break;
                    }
                }

            }
            else
            {
            }

            Debug.Log("angele__" + acceleration + "______" + angle);

        }
    }

    public void LoadToping(DataToping spr)
    {
        var toping = Instantiate(spr.type.Equals(TYPE_TOPING.DEFAULT) ? prefabToping : prefabTopingDa, null);
        var cpn = toping.GetComponent<TopingItem>();
        var size = cpn.sprImg.bounds.size;
        toping.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-180, 180));
        toping.transform.position = new Vector3(UnityEngine.Random.Range(DringCupManager.Instance.p1.position.x + size.x / 2, DringCupManager.Instance.p2.position.x - size.x / 2), SizeCamera.y / 2 + size.y / 2);
        cpn.SetData(spr.spr);
        cpn.OnUpdateUi();
        toping.transform.localScale = Vector3.one * 0.5f;
    }

    private IEnumerator CheckTimeLoadToping()
    {
        yield return new WaitForSeconds(1.5f);
        isAutoToping = false;
    }

    private IEnumerator onAutoLoadToping()
    {
        yield return new WaitForEndOfFrame();
        if (!isAutoToping)
        {
            yield break;
        }

        var count = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            var spr = dataToping[UnityEngine.Random.Range(0, dataToping.Count - 1)];
            LoadToping(spr);
        }

        if(!isAutoToping)
        {
            yield break;
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.35f));
        this.StartCoroutine(onAutoLoadToping());
    }

    public void OnChangeColorForVnClick()
    {
        objChangeColorNc.gameObject.SetActive(true);

        var cpn = objChangeColorNc.GetComponent<WDChangeColorVn>();

        cpn.SetActiviveSlide(numWater);
        cpn.sli1.callback = color => {
            water2D_Spawners[0].OnUpdateColor(color);
        };
        cpn.sli1.UpdateStateHandleRect(water2D_Spawners[0].GetHSVColor());

        if (water2D_Spawners.Count > 1)
        {
            cpn.sli2.callback = color => {
                water2D_Spawners[1].OnUpdateColor(color);

            };
            cpn.sli2.UpdateStateHandleRect(water2D_Spawners[1].GetHSVColor());
        }
    }

    public void OnBack()
    {
        Audio.StopBackgroundMusic();

#if UNITY_EDITOR
        HelperManager.OnLoadScene(ScStaticScene.HOME_SCENE);
#else
        UnityMessageManager.Instance.SendMessageToFlutter("back");
        UnityMessageManager.Instance.SendMessageToFlutter("sound_true");
#endif
    }

    public void OnShowFruit()
    {
        if(isRunAnimFruit)
        {
            return;
        }

        isRunAnimFruit = true;

        if (isEnableFruit)
        {
            objFruit.DOAnchorPosX(-200, 0.25f).SetEase(Ease.OutQuad).OnComplete(() => {
                objFruit.gameObject.SetActive(false);
                isEnableFruit = false;
                isRunAnimFruit = false;
            });
        }
        else
        {
            objFruit.gameObject.SetActive(true);
            objFruit.DOAnchorPosX(0, 0.25f).SetEase(Ease.OutQuad).OnComplete(() => {
                isEnableFruit = true;
                isRunAnimFruit = false;
            });
        }
    }   
    
    public void OnShowChangeCup()
    {
        if(isRunAnimChangeCup)
        {
            return;
        }

        isRunAnimChangeCup = true;

        if (isEnableChangCup)
        {
            objChangeCup.DOAnchorPosX(200, 0.25f).SetEase(Ease.OutQuad).OnComplete(() => {
                objChangeCup.gameObject.SetActive(false);
                isEnableChangCup = false;
                isRunAnimChangeCup = false;
            });

        }
        else
        {
            objChangeCup.gameObject.SetActive(true);
            objChangeCup.DOAnchorPosX(0, 0.25f).SetEase(Ease.OutQuad).OnComplete(() => {
                isEnableChangCup = true;
                isRunAnimChangeCup = false;
            });
        }
    }

    
}
