using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DringCupManager : SingletonMono<DringCupManager>
{
   public List<SpriteRenderer> sprCup = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color color)
    {
        foreach(var sp in sprCup)
        {
            sp.color = color;
        }
    }
}
