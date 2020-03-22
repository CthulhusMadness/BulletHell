using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    public static GameManager Instance;

    [SerializeField] private Agent player;

    private int enemiesAlive;

    public int EnemiesAlive
    {
        get => enemiesAlive;
        set => enemiesAlive = value;
    }

    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Methods

    public bool ControlIfSpawnedEnemiesAreDead()
    {
        if (enemiesAlive <= 0)
        {
            Debug.Log("Enemies are all dead");
            return true;
        }
        return false;
    }

    #endregion
}
