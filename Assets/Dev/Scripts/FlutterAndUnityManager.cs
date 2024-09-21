using System.Collections;
using System.Linq;
using UnityEngine;

public class FlutterAndUnityManager : MonoBehaviour
{
    [SerializeField] HomeScene homeScene;
    void Start()
    {

#if UNITY_EDITOR
        ScStaticScene.NumCup = 2;
#endif

        if (ScStaticScene.IsReplay)
        {
            homeScene.sttate = ScStaticScene.Message;
            homeScene.OnPlayGame();
            ScStaticScene.IsReplay = false;
        }

    }

    public void OnPlayGame(string message)
    {
        ScStaticScene.Message = message;
        ScStaticScene.IsReplay = true;
        HelperManager.OnLoadScene(ScStaticScene.HOME_SCENE);
    }
}
