using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightResizePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 minSize;
    public Vector2 maxSize;

    private RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;
    private float OldWidth;
    private float OldPosx;

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        rectTransform.SetAsLastSibling();
        OldWidth = rectTransform.sizeDelta.x;
        OldPosx = rectTransform.localPosition.x;
        Debug.LogError("Right Pivotx : " + OldWidth);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera,
            out previousPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera,
            out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

        sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);

        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
        );

        rectTransform.localPosition = new Vector2(OldPosx + ((sizeDelta.x - OldWidth) / 2), rectTransform.localPosition.y);
        rectTransform.sizeDelta = sizeDelta;
        previousPointerPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OldWidth = 0;
        OldPosx = 0;
    }
}