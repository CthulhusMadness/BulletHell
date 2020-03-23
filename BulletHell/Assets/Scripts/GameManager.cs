using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Fields

    public static GameManager Instance;

    [SerializeField] private Agent player;
    [SerializeField] private float resetWaitTime = 2f;

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

    public void PlayerDeath()
    {
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetWaitTime);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncLoad.allowSceneActivation = false;
        yield return (asyncLoad.progress > 0.9f);
        asyncLoad.allowSceneActivation = true;

    }

    #endregion
}
