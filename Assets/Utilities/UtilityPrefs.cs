using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtilities
{
    public class UtilityPrefs
    {
        public static string ScenePathName = "ScenePath";

        public static string ScenePath
        {
            get => PlayerPrefs.GetString(ScenePathName, "");
            set => PlayerPrefs.SetString(ScenePathName, value);
        }
    }
}