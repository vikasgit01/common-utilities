///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;


namespace GameManagers
{

    /// <summary>
    /// Use this class if you want to load a scene additively
    /// this class does is turns off the game objects with AdditiveSceneBase Attached to it
    /// For Example audio Listener / Camera / EventSystem etc.
    /// </summary>
    public class AdditiveSceneHandler : MonoBehaviour
    {
        [Tooltip("Name of the Scene This Script is Attached IN")]
        [SerializeField] private string _sceneName;
        [Tooltip("Attach GameObject AdditiveSceneObject.cs or your own Script with AdditiveSceneBase inherited")]
        [SerializeField] private AdditiveSceneBase[] _additiveObject;

        /// <summary>
        /// Check and sets AdditiveSceneBase GameObjects to false;
        /// </summary>
        void Start()
        {
            //Get if the current scene is Additively Loaded
            bool isSceneLoadedAdditively = (CustomScenesManager.Instance && CustomScenesManager.Instance.IsSceneLoadedAdditively(_sceneName));

            //function to Activate or De-active Additive SceneBase objects
            SetUpAdditiveData(!isSceneLoadedAdditively);
        }

        /// <summary>
        /// Function to Activate or De-active Additive SceneBase objects
        /// </summary>
        /// <param name="value"></param>
        void SetUpAdditiveData(bool value)
        {
            //if AdditiveObject is null return
            if (_additiveObject == null) return;

            //get length of the array
            int length = _additiveObject.Length;

            //loop and set objects to active or De-active
            for (int i = 0; i < length; i++)
            {
                if (_additiveObject[i] != null) _additiveObject[i].Show(value);
            }
        }
    }
}
