using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObjectWithScreen : MonoBehaviour
{
    public float distanceX = 0.0f;
    public float distanceY = 0.0f;

    public bool isNew = true;

    private void Start()
    {
        if(isNew)
        {
            ScaleParent();
        }
    }

    public float ScaleParent()
    {
        transform.localScale = Vector3.one;
        distanceX = 0.0f;
        var SizeScrren = HelperManager.GetSizeCamera();
        var sizeObject = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        float ScaleX = 1.0f;
        if (sizeObject.x > SizeScrren.x - distanceX * 2)
        {
            ScaleX = (SizeScrren.x - distanceX * 2) / sizeObject.x;
            transform.localScale = Vector3.one * (ScaleX);
        }

        return ScaleX;
    }

    public float ScaleParent(Transform p1, Transform p2)
    {
        distanceX = 0f;
        var y = Mathf.Abs(p1.position.y - p2.position.y);

        var SizeScrren = HelperManager.GetSizeCameraNew();

        var sizeObject = new Vector2(9.86f, 14.32f);
        float ScaleX = 1.0f;
        if (sizeObject.x > SizeScrren.x - distanceX * 2)
        {
            ScaleX = (SizeScrren.x - distanceX * 2) / sizeObject.x;
        }

        var ScaleY = y / (sizeObject.y - distanceY);
        var scale = Mathf.Min(ScaleX, ScaleY);
        transform.localScale = Vector3.one * (scale);
        return scale;
    }

   
}
