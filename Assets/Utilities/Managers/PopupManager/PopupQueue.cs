///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GameManagers
{
    /// <summary>
    /// class for SceneStack data
    /// </summary>
    public class SceneStack
    {
        //stack of scenes
        private Stack<PopupQueueData> sceneStack;
      
        /// <summary>
        /// function to show popup from Stack and show popup
        /// </summary>
        /// <param name="sceneAction">popup queue data to add to stack</param>
        public void ShowPopupFromQueue(PopupQueueData sceneAction)
        {
            AddToStack(sceneAction);
            sceneAction?.SetUp();
        }

        /// <summary>
        /// Function to add to stack
        /// </summary>
        /// <param name="popupAction">popup queue data to add to queue</param>
        public void AddToStack(PopupQueueData popupAction)
        {
            if (sceneStack == null) sceneStack = new Stack<PopupQueueData>();

            sceneStack?.Push(popupAction);
        }

        /// <summary>
        /// Close Scene from stack
        /// </summary>
        /// <returns>returns true if popup closed successfully </returns>
        public bool Close()
        {
            //check for sceneStack is not null and count is > 0 
            if (sceneStack != null && sceneStack.Count > 0)
            {
                //invoke PopupQueueData close function
                sceneStack?.First().Close();
                //can show next popup
                bool showNext = sceneStack.First().ShowNext;
                //pop out of stack
                sceneStack?.Pop();

                //if there count is 0 after removing the popup
                if (sceneStack.Count <= 0) return false;

                //setup next popup
                if (showNext && sceneStack.Count > 0) sceneStack?.First().SetUp();

                return true;
            }

            LogsManager.Print("#PopupQueue : No Data Game");
            return false;
        }

        /// <summary>
        /// reset scene stack
        /// </summary>
        public void ResetData() => sceneStack?.Clear();
    }

    /// <summary>
    /// class for PopupQueue data
    /// </summary>
    public class PopupQueue
    {
        /// <summary>
        /// Queue of popups
        /// </summary>
        private Queue<PopupQueueData> popupQueue;

        /// <summary>
        /// function to show popup from queue and show popup
        /// </summary>
        /// <param name="sceneAction">popup queue data to add to queue</param>
        public void ShowPopupFromQueue(PopupQueueData popupAction)
        {
            AddToQueue(popupAction);
            popupAction?.SetUp();
        }

        /// <summary>
        /// Function to add to queue
        /// </summary>
        /// <param name="popupAction">popup queue data to add to queue</param>
        public void AddToQueue(PopupQueueData poupAction)
        {
            if (popupQueue == null) popupQueue = new Queue<PopupQueueData>();

            popupQueue?.Enqueue(poupAction);
        }

        /// <summary>
        /// Close Scene from queue
        /// </summary>
        /// <returns>returns true if popup closed successfully </returns>
        public bool Close()
        {
            //check for popupQueue is not null and count is > 0 
            if (popupQueue != null && popupQueue.Count > 0)
            {
                //invoke PopupQueueData close function
                popupQueue?.First().Close();
                //can show next popup
                bool showNext = popupQueue.First().ShowNext;
                //dequeue out of queue
                popupQueue?.Dequeue();

                //setup next popup
                if (showNext && popupQueue.Count > 0) popupQueue?.First().SetUp();

                return true;
            }

            LogsManager.Print("#PopupQueue : No Data Game");
            return false;
        }

        /// <summary>
        /// reset scene stack
        /// </summary>
        public void ResetData() => popupQueue?.Clear();
    }

    /// <summary>
    /// Data class for Scene and Popups
    /// </summary>
    public class PopupQueueData
    {
        //variable to determine to show next popup if this is close
        public bool ShowNext;
        //if this is not null then pop is shown
        public GameObject Prefab;
        //if scene name is not null then sceneName is shown
        public string SceneName;

        //Interface that has close and show popup
        private IPopupActions popupActions;
        //store last scene name
        private string LastSceneName;

        //constructor to setup data
        public PopupQueueData(bool ShowNext, GameObject Prefab = null, string SceneName = "")
        {
            this.Prefab = Prefab;
            this.ShowNext = ShowNext;
            this.SceneName = SceneName;
            if (IsScene) LastSceneName = CustomScenesManager.Instance.GetCurrentSceneName();
        }

        /// <summary>
        /// function to setup or show the popup/scene
        /// </summary>
        public void SetUp()
        {
            LogsManager.Print($"#PopupQueueData : IsGameObject : {IsGameObject}, IsScene : {IsScene}");

            // check if this data is for popup
            if (IsGameObject)
            {
                popupActions = GameObject.Instantiate(Prefab).GetComponent<IPopupActions>();
                LogsManager.Print($"#PopupQueueData : popupActions : {popupActions != null}");
            }
            //check if this data is for scene
            else if (IsScene)
            {
                LogsManager.Print($"#PopupQueueData : SceneName : {SceneName} LastSceneName : {LastSceneName}");
                CustomScenesManager.Instance.LoadScene(SceneName);
            }
        }

        /// <summary>
        /// function to close popup/scene
        /// </summary>
        public void Close()
        {
            LogsManager.Print($"#PopupQueueData : Close, IsGameObject : {IsGameObject}, popupActions : {popupActions != null},SceneName : {SceneName},LastName : {LastSceneName}");
            //if Gameobject close
            if (IsGameObject) popupActions?.Close();
            //if scene show last scene this scene was loaded from.
            else if (!Prefab && !string.IsNullOrEmpty(LastSceneName))
                CustomScenesManager.Instance.LoadScene(LastSceneName);
        }

        //returns true if this is for popup
        bool IsGameObject => Prefab && string.IsNullOrEmpty(SceneName);
        //returns true if this is for a scene
        bool IsScene => !Prefab && !string.IsNullOrEmpty(SceneName) && CustomScenesManager.Instance;
    }

    public interface IPopupActions
    {
        public void SetUp();
        public void Close();
    }
}
