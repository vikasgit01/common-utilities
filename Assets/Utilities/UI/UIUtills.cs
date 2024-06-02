///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUIUtilities
{
    public class UIUtills : MonoBehaviour
    {
        public static bool IsCenterPivot(RectTransform rectTransform) => rectTransform.pivot != null && rectTransform.pivot == new Vector2(.5f, .5f);
        public static bool IsTopPivot(RectTransform rectTransform) => rectTransform.pivot != null && rectTransform.pivot == new Vector2(.5f, 1f);
        public static bool IsBottomPivot(RectTransform rectTransform) => rectTransform.pivot != null && rectTransform.pivot == new Vector2(.5f, 0f);
        public static bool IsLeftPivot(RectTransform rectTransform) => rectTransform.pivot != null && rectTransform.pivot == new Vector2(0f, .5f);
        public static bool IsRightPivot(RectTransform rectTransform) => rectTransform.pivot != null && rectTransform.pivot == new Vector2(1f, 0.5f);
    }
}
