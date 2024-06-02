using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using GameManagers;

namespace GameUtilities
{
    public class Environment : MonoBehaviour
    {
        [MenuItem("Utility/StartorStopGame")]
        [System.Obsolete]
        public static void StartAndStopGame()
        {
            if (EditorApplication.isPlaying == true)
            {
                EditorApplication.isPlaying = false;

                if (!string.IsNullOrEmpty(UtilityPrefs.ScenePath)) OpenScene(UtilityPrefs.ScenePath);
                return;
            }

            UtilityPrefs.ScenePath = EditorApplication.currentScene;
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene(SceneUtility.GetScenePathByBuildIndex(0));
            EditorApplication.isPlaying = true;
        }

        [System.Obsolete]
        async static void OpenScene(string name)
        {
            await System.Threading.Tasks.Task.Delay(500);
            EditorApplication.OpenScene(name);
        }

        [MenuItem("Utility/Environment/OpenPersistentDataPathFolder")]
        public static void OpenPersistentDataPathFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        [MenuItem("Utility/Environment/Logs/EnableLogs")]
        public static void EnableLogs() => AddScriptingDefinition(UtilitiesConstants.EnableLogs);
        [MenuItem("Utility/Environment/Logs/DisableLogs")]
        public static void DisableLogs() => RemoveScriptingDefinition(UtilitiesConstants.EnableLogs);
        [MenuItem("Utility/Environment/SwitchToLocal")]
        public static void SwitchToLocal() => AddScriptingDefinition(UtilitiesConstants.Dev);

        #region Scripting Definition
        public static void AddScriptingDefinition(string definition)
        {
            string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetTargetGroup());

            if (!def.Contains(definition))
            {
                LogsManager.Print("Logs Enabled");
                if (def.Length > 0)
                    def += ";";

                def += definition;

                PlayerSettings.SetScriptingDefineSymbolsForGroup(GetTargetGroup(), def);
                return;
            }

            LogsManager.Print("Logs are Already Enabled");
        }
        public static void RemoveScriptingDefinition(string definition)
        {
            string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(GetTargetGroup());

            if (def.Contains(definition))
            {
                int index = def.IndexOf(definition);

                if (index < 0)
                    return;
                else if (index > 0)
                    index -= 1;

                int lengthToRemove = System.Math.Min(definition.Length + 1, def.Length - index);

                def = def.Remove(index, lengthToRemove);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(GetTargetGroup(), def);
            }
        }
        public static BuildTargetGroup GetTargetGroup() => BuildTargetGroup.Android;
        #endregion

    }
}