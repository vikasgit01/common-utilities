using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagers
{

    [CreateAssetMenu(fileName = "SceneManagerData", menuName = "ScriptableObjects/SceneManagerData", order = 1)]
    public class SceneManagerData : ScriptableObject
    {
        public List<SceneLoader> scenes;
        public List<SceneLoader> GetScenes() => scenes;
    }


    [Serializable]
    public class SceneLoader
    {
        public LoadSceneMode SceneMode;
        public SceneAsset SceneAsset;

        private string sceneName;
        private string _sceneName
        {
            get
            {
                if(string.IsNullOrEmpty(sceneName) && SceneAsset != null)
                {
                    return SceneAsset.name;
                }
                else
                {
                    return sceneName;
                }
            }
        }
        private string additiveSceneName;

        public SceneLoader(string sceneName)
        {
            this.sceneName = sceneName;
        }

        public string GetSceneName() => _sceneName;

        public bool IsAdditiveLoadingScene
        {
            get => SceneMode == LoadSceneMode.Additive;
        }

        public Scene Scene
        {
            get => SceneManager.GetSceneByName(_sceneName);
        }

        public void LoadScene()
        {
            if (IsAdditiveLoadingScene)
            {
                additiveSceneName = _sceneName;
                SceneManager.sceneLoaded += OnAdditiveSceneLoaded;
            }else { additiveSceneName = ""; }

            SceneManager.LoadScene(_sceneName, SceneMode);
        }

        public bool UnloadScene()
        {
            if (Scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(_sceneName);
                return true;
            }
            return false;
        }

        private void OnAdditiveSceneLoaded(Scene scene,LoadSceneMode mode) 
        {
            try
            {
                if(scene.name == additiveSceneName)
                {
                    if(scene.isLoaded) SceneManager.SetActiveScene(scene);
                }
            }
            catch(Exception ex)
            {

            }

            additiveSceneName = "";
            SceneManager.sceneLoaded -= OnAdditiveSceneLoaded;
        }

    }

}
