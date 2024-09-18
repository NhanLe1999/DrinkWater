#region Includes
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

[RequireComponent(typeof(RectTransform))]
public class PointerTrackerArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables

    private const string TRACK_COROUTINE = "TrackCoroutine";

    public delegate void OnDrag(PointerTrackerArea sender, Vector2 position);
    public OnDrag Drag;

    private RectTransform _transform;
    [SerializeField] Canvas _parentCanvas;

    [SerializeField] private RectTransform _rectTrParent;

    #endregion

    private void Awake()
    {
        _transform = transform as RectTransform;
    }

    public Vector2 Normalize(Vector2 position)
    {
        return new Vector2(position.x / _transform.rect.width, (position.y - _rectTrParent.anchoredPosition.y - 50) / _transform.rect.height);
    }
    public Vector2 DeNormalize(Vector2 position)
    {
        return new Vector2(position.x * _transform.rect.width, position.y * _transform.rect.height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(TRACK_COROUTINE);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(TRACK_COROUTINE);
    }

    private IEnumerator TrackCoroutine()
    {
        while (true)
        {
            var position = HelperManager.ConvertWorldToCanvasPoint(_parentCanvas, Input.mousePosition);
            Debug.Log("ponit: " + position);
            var rect = _transform.rect;
            position.x = Mathf.Clamp(position.x, rect.min.x, rect.max.x);
            position.y = Mathf.Clamp(position.y, rect.min.y - _transform.rect.height, rect.max.y) ;

            Drag?.Invoke(this, position);

            yield return 0;
        }
    }
}