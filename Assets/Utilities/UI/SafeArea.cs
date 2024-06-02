using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtills{

public class SafeArea : MonoBehaviour
{
    [SerializeField] private float topmargin;
    public Transform Canvas;
    RectTransform Panel;
    Rect LastSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        Panel = this.GetComponent<RectTransform>();
        Refresh();
    }

    void Refresh()
    {
        Rect safeArea = GetSafeArea();

        if (safeArea != LastSafeArea)
        {
            ApplySafeArea(safeArea);
        }
        else
        {
            GetComponent<SafeArea>().enabled = false;
        }
    }

    Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    void ApplySafeArea(Rect r)
    {
        LastSafeArea = r;

        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;
        topNotchDeviceEvent?.Invoke(anchorMin.y.ToString().Equals("0.0") && anchorMin.y.ToString().Equals("1.0"));
    }

    private Action<bool> topNotchDeviceEvent;
    public void SetTopNotchDeviceEvent(Action<bool> action)
    {
        topNotchDeviceEvent = action;

    }
}

}
