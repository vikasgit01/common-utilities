///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using DesignPatterns;
using GameUtills;

namespace GameManagers
{
    public class PopupManager : Singleton<PopupManager>
    {
        private SceneStack sceneQueue;
        private PopupQueue inScenePopupQueue;

        protected override void Awake()
        {
            base.Awake();
            sceneQueue = new SceneStack();
            inScenePopupQueue = new PopupQueue();
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (inScenePopupQueue != null && inScenePopupQueue.Close())
                {

                }
                else if (sceneQueue != null && sceneQueue.Close())
                {
                    inScenePopupQueue?.ResetData();
                }
                else if (CustomScenesManager.Instance)
                {
                    CustomScenesManager.Instance.ExitGame();
                }
            }
        }

        public void LoadScene(string SceneName)
        {
            LogsManager.Print($"#PopupManager : LoadScene : SceneName - {SceneName}");
            SceneStack sceneQueue = GetScenePopupQueue();
            if (sceneQueue != null)
                sceneQueue.ShowPopupFromQueue(new PopupQueueData(false, SceneName: SceneName));
        }

        public void CloseScenes()
        {
            if (sceneQueue != null && sceneQueue.Close())
                inScenePopupQueue?.ResetData();
            else if (CustomScenesManager.Instance)
                CustomScenesManager.Instance.ExitGame();
        }

        public void LoadGameObect(GameObject go, bool showPopup)
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

        public void ClosePopupFromQueue()
        {
            if (inScenePopupQueue != null && inScenePopupQueue.Close())
            {

            }
        }

        public SceneStack GetScenePopupQueue()
        {
            inScenePopupQueue?.ResetData();
            return sceneQueue;
        }
        public PopupQueue GetInScenePopupQueue() => inScenePopupQueue;
    }
}
