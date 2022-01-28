using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeSetter : MonoBehaviour
{
    public bool right;
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        // float h = canvas.GetComponent<RectTransform>().rect.height;
        float w = canvas.GetComponent<RectTransform>().rect.width;
        // Debug.LogError("ScreenWidth : "+w);
        // Debug.LogError("ScreenWidth/2 : "+w/2);
        // transform.GetComponent<RectTransform>().sizeDelta = new Vector2(right?(w/2)-5:(w/2)-1,0);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(w/2,0);
    }
    
}
