using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DringCupManager : SingletonMono<DringCupManager>
{
    public List<SpriteRenderer> sprCup = null;
    public Transform p1 = null;
    public Transform p2 = null;

    void Start()
    {
        p1 = transform.Find("p1");
        p2 = transform.Find("p2");

        if (p1.transform.position.x >= p2.transform.position.x)
        {
            p1 = transform.Find("p2");
            p2 = transform.Find("p1");
        }

        foreach (var spr in sprCup)
        {
            HelperManager.SetLayerRecursively(spr.gameObject, "Cup");
        }

       // Destroy(gameObject.GetComponent<Rigidbody2D>());
    }

    

    public void SetColor(Color color)
    {
        foreach(var sp in sprCup)
        {
            sp.color = color;
        }
    }

    public void SetScaleForCup()
    {

    }
}
