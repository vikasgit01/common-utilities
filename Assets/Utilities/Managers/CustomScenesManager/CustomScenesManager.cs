///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DesignPatterns;

namespace GameManagers
{
    /// <summary>
    /// This is a singleton class can be used to load scenes Normal, Additive and unload scene
    /// you need to create a scriptable from "Assets/Create/ScriptableObjects/SceneManagerData" to config the sceneData
    /// </summary>
    public class CustomScenesManager : Singleton<CustomScenesManager>
    {
        [Tooltip("Attach AudioManagerData Scriptable here with data configured as required")]
        [SerializeField] private SceneManagerData _sceneManagerData;

        //used for additive scene loading to keep track of all the additive scenes 
        private Stack<Scene> _previousAdditiveScenes;


        /// <summary>
        /// protected unity function
        /// called only once 
        /// </summary>
        protected override void Awake()
        {
            //calling function from base class
            base.Awake();

            //setup data for scenes
            SetUpScenes();

            //Keep this instance thorought game
            DontDestroyOnLoad(this);
        }

        //add all the scenes to the Dictionary.
        private Dictionary<string, SceneLoader> allScenes;

        /// <summary>
        /// Setup Data into Dictionary
        /// </summary>
        public void SetUpScenes()
        {
            //create new Dic if its null
            if (allScenes == null) allScenes = new Dictionary<string, SceneLoader>();

            //if scene Data is not available use scenes from build settings
            if (_sceneManagerData == null || _sceneManagerData.GetScenes() == null || _sceneManagerData.GetScenes().Count <= 0)
            {
                int count = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < count; i++)
                {
                    string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    LogsManager.Print($"Scene Added To SceneManager Dictionary : {sceneName}");

                    if(!allScenes.ContainsKey(sceneName))
                        allScenes.Add(sceneName, new SceneLoader(sceneName));
                }
            }
            else
            {
                int count = _sceneManagerData.GetScenes().Count;
                for (int i = 0; i < count; i++)
                {
                    string sceneName = _sceneManagerData.GetScenes()[i].GetSceneName();
                    LogsManager.Print($"Scene Added To SceneManager Dictionary : {sceneName}");

                    if (!allScenes.ContainsKey(sceneName))
                        allScenes.Add(sceneName, _sceneManagerData.GetScenes()[i]);
                }
            }
        }

        /// <summary>
        /// Function to load scene
        /// </summary>
        /// <param name="sceneName">Name of the scene to Load</param>
        /// <param name="updateActiveScene">if true keeps track of additive scenes loaded if scene reqested to load is additive</param>
        public void LoadScene(string sceneName,bool updateActiveScene = true)
        {
            //check If scene exits in Dic 
            if (allScenes != null && allScenes.ContainsKey(sceneName))
            {
                //store scene loader in local variable
                SceneLoader sceneLoader = allScenes[sceneName];


                if (updateActiveScene && sceneLoader.IsAdditiveLoadingScene)
                {
                    //Do Additive Loading Stuff
                    AddSceneToPreviousAdditiveScenes(SceneManager.GetActiveScene());
                }
                else
                {
                    ResetPreviousAdditiveScenes();
                }

                LoadScene(sceneLoader);
            }
            else
            {
                Exception ex = new Exception($"Scene is not in the Dictionary : {sceneName}");
                LogsManager.PrintException(ex);
            }
        }

        /// <summary>
        /// Function to unload scene
        /// </summary>
        /// <param name="unloadSceneName">Name of the scene to Unload</param>
        public void UnloadScene(string unloadSceneName)
        {
            //check If scene exits in Dic 
            if (allScenes != null && allScenes.ContainsKey(unloadSceneName))
            {
                //store scene loader in local variable
                SceneLoader sceneLoader = allScenes[unloadSceneName];

                if (sceneLoader.IsAdditiveLoadingScene)
                {
                    //Do Additive Unloading Stuff
                    RemovePreviousScene();
                }

                UnLoadScene(sceneLoader);
            }
            else
            {
                Exception ex = new Exception($"Scene is not in the Dictionary : {unloadSceneName}");
                LogsManager.PrintException(ex);
            }
        }

        /// <summary>
        /// Function to pause the game
        /// </summary>
        /// <param name="pause">true - Pauses, false - resumes</param>
        public void PauseGame(bool pause)
        {
            LogsManager.Print($"Pause Game : {pause}");
            Time.timeScale = pause ? 0f : 1.0f;
        }

        /// <summary>
        /// Function to Exit the game
        /// </summary>
        public void ExitGame()
        {
            LogsManager.Print($"Quit Game");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Check if the scene is Additive
        /// </summary>
        /// <param name="sceneName">name to check</param>
        /// <returns></returns>
        public bool IsSceneLoadedAdditively(string sceneName)
        {
            if (allScenes != null && allScenes.ContainsKey(sceneName))
                return allScenes[sceneName].IsAdditiveLoadingScene;

            return false;
        }

        //load a scene
        void LoadScene(SceneLoader sceneLoader) => sceneLoader.LoadScene();

        //unload a scene
        void UnLoadScene(SceneLoader sceneLoader) => sceneLoader.UnloadScene();

        //all additive scenes to stack
        void AddSceneToPreviousAdditiveScenes(Scene scene)
        {
            try
            {
                if (_previousAdditiveScenes == null) _previousAdditiveScenes = new Stack<Scene>();

                if(!_previousAdditiveScenes.Contains(scene))
                    _previousAdditiveScenes.Push(scene);
            }
            catch (Exception ex)
            {

            }
        }

        //reset additive stack
        void ResetPreviousAdditiveScenes()
        {
            if(_previousAdditiveScenes != null) _previousAdditiveScenes.Clear();
        }

        //remove previous scene from stack
        void RemovePreviousScene()
        {
            try
            {
                if(_previousAdditiveScenes !=null && _previousAdditiveScenes.Count > 0)
                {
                    Scene previousScene = _previousAdditiveScenes.Pop();
                    
                    if(previousScene.isLoaded)
                        SceneManager.SetActiveScene(previousScene);
                }
            }catch (Exception ex)
            {

            }
        }

        //get current scene name
        public string GetCurrentSceneName() => SceneManager.GetActiveScene().name;
    }

}