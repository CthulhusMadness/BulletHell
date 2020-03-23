using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXManager", menuName = "SFX/Manager")]
public class SFXManager : SerializedSingletonSO<SFXManager>
{
    #region Fields

    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = true, KeyLabel = "Key", ValueLabel = "SFXData")]
    public Dictionary<string, SFXData> SFXDictionary = new Dictionary<string, SFXData>();
    [PropertySpace(10)]
    [SerializeField] private string[] keys;
    
    #endregion

    #region Methods

    public SFXData GetSFXDataFromKey(string sfxKey)
    {
        return SFXDictionary[sfxKey];
    }

    [Button(ButtonSizes.Medium, Name = "Set Keys")]
    private void SetKeysToDictionary()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (!SFXDictionary.ContainsKey(keys[i]))
            {
                SFXDictionary.Add(keys[i], null);
            }
        }
        
        List<string> keysToDelete = new List<string>();
        
        foreach (string key in SFXDictionary.Keys)
        {
            if (System.Array.IndexOf(keys, key) == -1)
            {
                keysToDelete.Add(key);
            }
        }

        for (int i = 0; i < keysToDelete.Count; i++)
        {
            if (SFXDictionary.ContainsKey(keysToDelete[i]))
            {
                SFXDictionary.Remove(keysToDelete[i]);
            }
        }
    }

    #endregion
}
