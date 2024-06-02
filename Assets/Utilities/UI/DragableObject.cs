using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableObject : MonoBehaviour, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;

    public void Start()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
      if(rectTransform)  rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
