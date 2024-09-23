using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonClick : MonoBehaviour
{
    void Start()
    {
        var btn = gameObject.GetComponent<Button>();
        if(btn != null)
        {
            btn.onClick.AddListener(onClickButton);
        }
    }

    void onClickButton()
    {
        Audio.Play(ScStaticScene.SFX_Click);
    }    
}
