using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WDWoodFloat : MonoBehaviour
{

    public float buoyancyForce = 105f;  
    public float sinkingSpeed = 2f;    
    public float maxFloatHeight = 2.0f; 
    private Rigidbody2D rb;            
    private bool isInMetaball_liquid = false;

    bool isMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Metaball_liquid")
        {
            isInMetaball_liquid = true; 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(isMove)
        {
            return;
        }


        if (collision.collider.tag == "Metaball_liquid")
        {
            if(collision.collider.transform.position.y - 0.25f > transform.position.y)
            {
                isMove = true;

                transform.DOMoveY(collision.collider.transform.position.y, 0.7f).OnComplete(() => {
                    isMove = false;
                });
            }


        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Metaball_liquid")
        {
            isInMetaball_liquid = false;  
        }
    }

    void FixedUpdate()
    {
       /* if (isInMetaball_liquid)
        {
            if (transform.position.y < maxFloatHeight)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(-sinkingSpeed, rb.velocity.y));

                rb.AddForce(Vector2.up * buoyancyForce * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else
            {
                
            }
        }*/
    }
}
