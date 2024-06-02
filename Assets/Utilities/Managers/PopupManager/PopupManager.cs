///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using DesignPatterns;

namespace GameManagers
{
    /// <summary>
    /// This Class is user to Manager Queue and Add popups to stack
    /// </summary>
    public class PopupManager : Singleton<PopupManager>
    {
        //this stack is to store the scenes opened
        private SceneStack sceneStack;

        //this Queue is to store popup in Queue
        private PopupQueue inScenePopupQueue;

        /// <summary>
        /// protected unity function
        /// called only once
        /// </summary>
        protected override void Awake()
        {
            //calls base class function
            base.Awake();

            //initialize stack
            sceneStack = new SceneStack();
            //initialize queue
            inScenePopupQueue = new PopupQueue();

            //Keep this instance thorough out the game
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// private unity functioned
        /// called every frame
        /// </summary>
        private void Update()
        {
            //used to check back event
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //close the popup from queue if any
                if (inScenePopupQueue != null && inScenePopupQueue.Close())
                {

                }
                //close the scenes from stack if any
                else if (sceneStack != null && sceneStack.Close())
                {
                    inScenePopupQueue?.ResetData();
                }
                //if no popup or scene exits in queue and stack then exit game
                else if (CustomScenesManager.Instance)
                {
                    CustomScenesManager.Instance.ExitGame();
                }
            }
        }

        /// <summary>
        /// Function to LoadScene and add to stack
        /// </summary>
        /// <param name="SceneName">name of the scene to load</param>
        public void LoadScene(string SceneName)
        {
            LogsManager.Print($"#PopupManager : LoadScene : SceneName - {SceneName}");
            //clear popup queue before opening the scene 
            SceneStack sceneQueue = GetScenePopupQueue();

            //add scene to sceneQueue
            if (sceneQueue != null)
                sceneQueue.ShowPopupFromQueue(new PopupQueueData(false, SceneName: SceneName));
        }

        /// <summary>
        /// Function to LoadGameObject and add to Queue
        /// </summary>
        /// <param name="go">GameObject to be Instantiated/Added</param>
        /// <param name="showPopup">true : show Popup Now, false : add to queue</param>
        public void LoadGameObject(GameObject go, bool showPopup)
        {
            PopupQueue popupQueue = GetInScenePopupQueue();
            if (popupQueue != null)
            {
                if (showPopup)
                    popupQueue.ShowPopupFromQueue(new PopupQueueData(true, go));
                else
                    popupQueue.AddToQueue(new PopupQueueData(true, go));
            }
        }

        /// <summary>
        /// Function to close current active scene from stack 
        /// </summary>
        public void CloseScenes()
        {
            if (sceneStack != null && sceneStack.Close())
                inScenePopupQueue?.ResetData();
            else if (CustomScenesManager.Instance)
                CustomScenesManager.Instance.ExitGame();
        }

        /// <summary>
        /// Function to close popups from queue
        /// </summary>
        public void ClosePopupFromQueue()
        {
            if (inScenePopupQueue != null && inScenePopupQueue.Close())
            {

            }
        }

        /// <summary>
        /// Function resets data of popup queue and return scene stack
        /// </summary>
        /// <returns>scene stack</returns>
        public SceneStack GetScenePopupQueue()
        {
            inScenePopupQueue?.ResetData();
            return sceneStack;
        }

        /// <summary> 
        /// Function returns popups
        /// </summary>
        /// <returns>popup queue</returns>
        public PopupQueue GetInScenePopupQueue() => inScenePopupQueue;
    }
}
