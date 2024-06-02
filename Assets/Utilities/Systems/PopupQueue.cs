using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GameManagers;

namespace GameUtills
{
    public class SceneStack
    {
        private Stack<PopupQueueData> sceneQueue;
      
        public void ShowPopupFromQueue(PopupQueueData sceneAction)
        {
            AddToQueue(sceneAction);
            sceneAction?.SetUp();
        }

        public void AddToQueue(PopupQueueData poupAction)
        {
            if (sceneQueue == null) sceneQueue = new Stack<PopupQueueData>();

            sceneQueue?.Push(poupAction);
        }

        public bool Close()
        {
            if (sceneQueue != null && sceneQueue.Count > 0)
            {
                sceneQueue?.First().Close();
                bool showNext = sceneQueue.First().ShowNext;
                sceneQueue?.Pop();

                if (sceneQueue.Count <= 0) return false;

                if (showNext && sceneQueue.Count > 0) sceneQueue?.First().SetUp();

                return true;
            }

            LogsManager.Print("#PopupQueue : No Data Game");
            return false;
        }

        public void ResetData() => sceneQueue?.Clear();
    }

    public class PopupQueue
    {
        private Queue<PopupQueueData> popupQueue;

        public void ShowPopupFromQueue(PopupQueueData popupAction)
        {
            AddToQueue(popupAction);
            popupAction?.SetUp();
        }

        public void AddToQueue(PopupQueueData poupAction)
        {
            if (popupQueue == null) popupQueue = new Queue<PopupQueueData>();

            popupQueue?.Enqueue(poupAction);
        }

        public bool Close()
        {
            if (popupQueue != null && popupQueue.Count > 0)
            {
                popupQueue?.First().Close();
                bool showNext = popupQueue.First().ShowNext;
                popupQueue?.Dequeue();

                if (showNext && popupQueue.Count > 0) popupQueue?.First().SetUp();

                return true;
            }

            LogsManager.Print("#PopupQueue : No Data Game");
            return false;
        }

        public void ResetData() => popupQueue?.Clear();
    }

    public class PopupQueueData
    {
        public bool ShowNext;
        public GameObject Prefab;
        public string SceneName;

        private IPopupActions popupActions;
        private string LastSceneName;
        public PopupQueueData(bool ShowNext, GameObject Prefab = null, string SceneName = "")
        {
            this.Prefab = Prefab;
            this.ShowNext = ShowNext;
            this.SceneName = SceneName;
            if (IsScene) LastSceneName = CustomScenesManager.Instance.GetCurrentSceneName();
        }

        public void SetUp()
        {
            LogsManager.Print($"#PopupQueueData : IsGameObject : {IsGameObject}, IsScene : {IsScene}");


            if (IsGameObject)
            {
                popupActions = GameObject.Instantiate(Prefab).GetComponent<IPopupActions>();
                LogsManager.Print($"#PopupQueueData : popupActions : {popupActions != null}");
            }
            else if (IsScene)
            {
                LogsManager.Print($"#PopupQueueData : SceneName : {SceneName} LastSceneName : {LastSceneName}");
                CustomScenesManager.Instance.LoadScene(SceneName);
            }
        }

        public void Close()
        {
            LogsManager.Print($"#PopupQueueData : Close, IsGameObject : {IsGameObject}, popupActions : {popupActions != null},SceneName : {SceneName},LastName : {LastSceneName}");
            if (IsGameObject) popupActions?.Close();
            else if (!Prefab && !string.IsNullOrEmpty(LastSceneName))
                CustomScenesManager.Instance.LoadScene(LastSceneName);
        }

        bool IsGameObject => Prefab && string.IsNullOrEmpty(SceneName);
        bool IsScene => !Prefab && !string.IsNullOrEmpty(SceneName) && CustomScenesManager.Instance;
    }

    public interface IPopupActions
    {
        public void SetUp();
        public void Close();
    }
}
