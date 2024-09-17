using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDScaleObjectWithCanvas : MonoBehaviour
{
    [SerializeField] bool isWidh = true;
    [SerializeField] Canvas canvas;
    [SerializeField] float xAdd = 0.0f;
    void Start()
    {
        var siseCanvas = HelperManager.GetSizeOfCanvas(canvas);
        var rectTr = gameObject.GetComponent<RectTransform>();
        var size = rectTr.rect;

        if(isWidh)
        {
            var scale = siseCanvas.x / (size.width + xAdd);
            rectTr.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            var scale = siseCanvas.y / size.height;
            rectTr.localScale = new Vector3(scale, scale, scale);
        }
    }
}
