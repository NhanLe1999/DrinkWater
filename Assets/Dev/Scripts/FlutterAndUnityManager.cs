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

    public void OnOFFSound(string isEnable)
    {
        PlayerPrefs.SetString(ScStaticScene.KEY_SAVE_SOUND, isEnable);
    }


    public void OnOFFMusic(string isEnable)
    {
        PlayerPrefs.SetString(ScStaticScene.KEY_SAVE_MUSIC, isEnable);

        if(isEnable.Equals("false"))
        {
            Audio.StopBackgroundMusic();
        }
        else
        {
            Audio.PlayBackgroundMusic(ScStaticScene.SFX_Music_Game);
        }
    }
}
