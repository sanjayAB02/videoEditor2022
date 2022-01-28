using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LeftResizePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector2 minSize;
    public Vector2 maxSize;

    private RectTransform rectTransform;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;

    private float OldWidth;
    private float OldPosx;

    public bool IsLeft;
    private float LeftDiffrent = 0;
    private float RightDiffrent = 0;
    private float LeftCounter = 0;
    private float RightCounter = 0;
    private Vector2 resizeValue;


    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        rectTransform.SetAsLastSibling();
        OldWidth = rectTransform.sizeDelta.x;
        OldPosx = rectTransform.localPosition.x;

        if (IsLeft)
        {
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
        }
        else
        {
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
            transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0.5f);
        }


        // Debug.LogError("Left OldWidth : "+OldWidth);
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

        resizeValue = currentPointerPosition - previousPointerPosition;

        if (resizeValue.x > 100)
        {
            return;
        }

        if (IsLeft)
        {
            sizeDelta -= new Vector2(resizeValue.x, -resizeValue.y);
        }
        else
        {
            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
        }


        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),rectTransform.sizeDelta.y
            // Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
        );


        if (resizeValue.x > 0)
        {
            //right side move
            if (!IsLeft && RightDiffrent <= 0)
            {
                Debug.LogError("Return 0 Move Right Side Right: " + RightDiffrent);
                return;
            }
        }
        else
        {
            //leftsidemove
            if (IsLeft && LeftDiffrent <= 0)
            {
                Debug.LogError("Return 0 Move Left Side Left : " + LeftDiffrent);
                return;
            }
        }
        


        if (IsLeft)
        {
            rectTransform.localPosition =
                new Vector2(OldPosx - ((sizeDelta.x - OldWidth) / 2), rectTransform.localPosition.y);
        }
        else
        {
            rectTransform.localPosition =
                new Vector2(OldPosx + ((sizeDelta.x - OldWidth) / 2), rectTransform.localPosition.y);
        }


        rectTransform.sizeDelta = sizeDelta;
        transform.parent.GetChild(2).GetChild(0).GetComponent<VideoTimeline>()
            .UpdateTimer(maxSize.x, sizeDelta.x, IsLeft);
        previousPointerPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        // if (resizeValue.x > 0)
        // {
        //right side move
        if (IsLeft)
        {
            LeftDiffrent += OldWidth - rectTransform.sizeDelta.x;
            Debug.LogError("LeftDiffrent : " + LeftDiffrent);
        }
        else
        {
            RightDiffrent += OldWidth - rectTransform.sizeDelta.x;
            Debug.LogError("RightDiffrent : " + RightDiffrent);
        }
        // }

        OldWidth = 0;
        OldPosx = 0;
        // else
        // {
        //     //leftsidemove
        //     if (IsLeft)
        //     {
        //         LeftCounter = -Mathf.Max(maxSize.x - sizeDelta.x,0); 
        //         Debug.LogError("Move Left Side Left : "+LeftCounter);
        //     }
        //     else
        //     {
        //         RightCounter = -Mathf.Max(maxSize.x - sizeDelta.x,0); 
        //         Debug.LogError("Move Left Side Right : "+RightCounter);
        //     }
        // }
        
    }
}