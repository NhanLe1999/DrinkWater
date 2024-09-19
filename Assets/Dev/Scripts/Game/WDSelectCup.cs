using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDSelectCup : SingletonMono<WDSelectCup>
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
