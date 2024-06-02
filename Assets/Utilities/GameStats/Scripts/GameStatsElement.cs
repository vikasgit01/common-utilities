using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStatsElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _textName;

    public void SetUp(string textName)
    {
        if (_textName) _textName.text = textName;
    }
}
