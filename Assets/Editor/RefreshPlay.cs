using UnityEditor;

namespace Editor
{
    public static class RefreshPlay
    {
        [InitializeOnLoadMethod]
        private static void Run()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    EditorApplication.ExecuteMenuItem("Assets/Refresh");
                }
            };
        }
    }
}