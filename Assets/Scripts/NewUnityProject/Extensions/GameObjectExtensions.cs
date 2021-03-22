using UnityEngine;

namespace NewUnityProject.Extensions
{
    public static class GameObjectExtensions
    {
        public static void RemoveComponent<T>(this GameObject gameObject) where T : Object
        {
            var component = gameObject.GetComponent<T>();
            Object.DestroyImmediate(component, true);
        }

        public static void RemoveAllComponents<T>(this GameObject gameObject) where T : Object
        {
            foreach (var component in gameObject.GetComponents<T>())
            {
                Object.DestroyImmediate(component, true);
            }
        }
    }
}