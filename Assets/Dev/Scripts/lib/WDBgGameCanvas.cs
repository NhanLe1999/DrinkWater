using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WDBgGameCanvas : MonoBehaviour
{
    [SerializeField] Canvas _canvas;

    [SerializeField] bool IsUseMaxSize = true;

    void Start()
    {
        var sizeCanvas = HelperManager.GetSizeOfCanvas(_canvas);

        var rectTransfrom = gameObject.GetComponent<RectTransform>();

        var sizeBg = rectTransfrom.rect;

        var scaleX = sizeCanvas.x / sizeBg.width;
        var scaleY = sizeCanvas.y / sizeBg.height;

        var scale = IsUseMaxSize ? Mathf.Max(scaleX, scaleY) : Mathf.Min(scaleX, scaleY);

        if(!IsUseMaxSize && (scaleX < 1.0f || scaleY < 1.0f) || IsUseMaxSize)
        {
            rectTransfrom.localScale = new Vector3(scale, scale, scale);
        }

    }

  
}
