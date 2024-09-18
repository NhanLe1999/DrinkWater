﻿#region Includes
using UnityEngine;
#endregion

[RequireComponent(typeof(PointerTrackerArea))]
public class RectPicker : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private RectTransform _handle;

    public delegate void OnValueChanged(RectPicker sender, Vector2 position);
    public OnValueChanged ValueChanged;

    public Vector2 Value
    {
        get { return Vector2.one; }
        set { }
    }
    public Vector2 NormalizedValue
    {
        get { return _tracker.Normalize(Value); }
        set { Value = _tracker ? _tracker.DeNormalize(value) : Vector2.zero; }
    }

    private PointerTrackerArea _tracker;

    #endregion

    private void Awake()
    {
        _tracker = GetComponent<PointerTrackerArea>();
        _tracker.Drag = PointerTrackerArea_Drag;
    }

    private void PointerTrackerArea_Drag(PointerTrackerArea sender, Vector2 position)
    {
        _handle.transform.localPosition = position;

        ValueChanged?.Invoke(this, position);
    }
}