using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeObjectWithCanvas : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform rectTransformTr;
    [SerializeField] float y = 291;
    void Start()
    {
        var rectTransForm = GetComponent<RectTransform>();
        var sizeCanvas = HelperManager.GetSizeOfCanvas(canvas);

        if(rectTransForm.sizeDelta.y + y > sizeCanvas.y)
        {
            var newXSize = sizeCanvas.y - y;
            rectTransForm.sizeDelta = new Vector2(rectTransForm.sizeDelta.x, newXSize);
        }
        rectTransformTr.sizeDelta = new Vector2(rectTransformTr.sizeDelta.x, rectTransForm.sizeDelta.y - 151);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
