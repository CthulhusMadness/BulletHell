using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SerializedSingletonSO<T> : SerializedScriptableObject where T : SerializedScriptableObject
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            }
            
#if UNITY_EDITOR
            if (!instance)
            {
                string[] configsGUIDs = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);
                if (configsGUIDs.Length > 0)
                {
                    instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(configsGUIDs[0]));
                }
            }
#endif
            
            return instance;
        }
        set => instance = value;
    }
}
