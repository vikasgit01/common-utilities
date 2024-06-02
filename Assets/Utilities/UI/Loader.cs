///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameUIUtilities
{

public class Loader : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;


    public void ShowLoading(bool show)
    {
        if (_rectTransform)
            _rectTransform.gameObject.SetActive(show);
    }

}
}
