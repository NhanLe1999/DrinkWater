using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class HelperManager
{
    public static void SetLayerRecursively(GameObject obj, string nameLayer)
    {
        if (obj == null)
        {
            return;
        }

        int layer = LayerMask.NameToLayer(nameLayer);
        if (layer == -1)
        {
            Debug.LogError("Layer " + nameLayer + " does not exist.");
            return;
        }
        obj.layer = layer;
    }

    public static void SetSortingLayerForSpriteRenderer(SpriteRenderer sprite, string sortingLayerName, int orderInLayer = 0)
    {
        if (sprite != null)
        {
            sprite.sortingLayerName = sortingLayerName;
            sprite.sortingOrder = orderInLayer;
        }
    }

    public static void SetSortingLayerForGroup(SortingGroup group, string sortingLayerName, int orderInLayer = 0)
    {
        if (group != null)
        {
            group.sortingLayerName = sortingLayerName;
            group.sortingOrder = orderInLayer;
        }
    }

    public static Vector2 GetSizeObjectInCanvas(RectTransform transformObj, Canvas canvas)
    {
        Vector3[] corners = new Vector3[4];
        transformObj.GetWorldCorners(corners);

        Vector2 screenSpaceMin = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[0]);
        Vector2 screenSpaceMax = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[2]);

        float objectWidth = screenSpaceMax.x - screenSpaceMin.x;
        float objectHeight = screenSpaceMax.y - screenSpaceMin.y;

        return new Vector2(objectWidth, objectHeight);
    }

    public static Vector2 ConvertPointCanvas(Camera camera, Canvas overlayCanvas, Vector3 cameraPoint, Camera cam1)
    {
        Vector2 screenPoint = camera.WorldToScreenPoint(cameraPoint);
        RectTransform canvasRect = overlayCanvas.GetComponent<RectTransform>();
        Vector2 overlayCanvasPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, cam1, out overlayCanvasPoint);

        return overlayCanvasPoint;
    }

    public static Vector2 ConvertWorldToCanvasPoint(Canvas canvas, Vector3 worldPosition)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        Vector2 canvasPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, Camera.main, out canvasPoint);

        return canvasPoint;
    }

    public static Vector2 GetSizeCamera()
    {
        Camera mainCamera = Camera.main;
        var height = mainCamera.orthographicSize * 2;
        var width = (float)Screen.width / (float)Screen.height * height;

        return new Vector2(width, height);
    }

    public static Vector2 GetSizeCameraNew()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenBottomLeft = new Vector3(0, 0, mainCamera.nearClipPlane); 
        Vector3 screenTopRight = new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane); 
        Vector3 worldBottomLeft = mainCamera.ScreenToWorldPoint(screenBottomLeft);
        Vector3 worldTopRight = mainCamera.ScreenToWorldPoint(screenTopRight);
        float width = worldTopRight.x - worldBottomLeft.x;
        float height = worldTopRight.y - worldBottomLeft.y;
        return new Vector2(width, height);
    }

    public static Vector2 GetSizeOfCanvas(Canvas canvas)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float width = canvasRect.rect.width;
        float height = canvasRect.rect.height;
        return new Vector2(width, height);
    }

    #region DATE TIME
    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    #endregion

    #region LOAD_SCENE
    public static void OnLoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    async public static UniTask LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            await UniTask.Yield();
        }
    }

    #endregion

}


