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
       
       // Destroy(gameObject.GetComponent<Rigidbody2D>());
    }

    public void Init()
    {

        transform.localScale = Vector3.one;

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
    }

    private IEnumerator DelayScale()
    {
        yield return new WaitForEndOfFrame();
       
    }

    public void UpdateScale()   
    {
        if (LogicGame.Instance.pointcheckCup.y - 0.25f < p1.transform.position.y)
        {
            var dis1 = LogicGame.Instance.pointcheckCup.y - 0.25f - transform.position.y;
            var dis2 = p1.transform.position.y - transform.position.y;
            float scale = Mathf.Abs(dis1 / dis2) * transform.localScale.x;

            var size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
            float sceleX = 1.0f;

            if (size.x > LogicGame.Instance.SizeCamera.x - 0.15f)
            {
                sceleX = (LogicGame.Instance.SizeCamera.x - 0.15f) / size.x;
            }

            transform.localScale = Vector3.one * Mathf.Min(sceleX, scale);
        }
        this.StartCoroutine(DelayScale());
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
