using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Fields

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject instance = Instantiate(pool.prefab);
                instance.name = "Projectile " + i;
                instance.SetActive(false);
                objectPool.Enqueue(instance);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    #endregion

    #region Methods

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"tag {tag} does not exist in the Pool Dictionary");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        
        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

    #endregion
}
