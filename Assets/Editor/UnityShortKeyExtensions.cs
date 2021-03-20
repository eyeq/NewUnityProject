using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class UnityShortKeyExtensions : ScriptableObject
    {
        [MenuItem("HotKey/Run _a")]
        private static void PlayGame()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        [MenuItem("HotKey/Stop (or play) _s")]
        private static void StopOrPlay()
        {
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        [MenuItem("HotKey/Refresh Assets _F5")]
        private static void DoRefresh()
        {
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        }
    }
}