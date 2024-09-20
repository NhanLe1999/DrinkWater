using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TopingItem : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] public SpriteRenderer sprImg;


    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if(transform.position.y < LogicGame.Instance.TrsObj.position.y)
        {
            Destroy(gameObject);
        }

    }

    public void SetData(Sprite spr)
    {
        sprImg.sprite = spr;
    }

    public void OnUpdateUi()
    {
        boxCollider2D.size = sprImg.bounds.size * 0.5f;
    }
}
