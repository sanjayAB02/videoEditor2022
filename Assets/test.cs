using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogError("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogError("OnDrag");
        _rectTransform.anchoredPosition += eventData.delta;
    }
}
