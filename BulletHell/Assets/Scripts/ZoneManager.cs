using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    #region Fields
    
    [Serializable]
    public class Wave
    {
        public List<GameObject> enemies = new List<GameObject>();
        public bool hasWaveTimer = false;
        [ShowIf("hasWaveTimer")] 
        public float timer = 10f;

        public void SpawnWave()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].SetActive(true);
            }
        }
    }
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private Color gizmoColor = Color.green;

    private GameManager gameManager;
    private bool canSpawnNewWave;
    private bool isWaveActivated = false;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isWaveActivated)
        {
            isWaveActivated = true;
            StartCoroutine(WaveControl());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    #endregion

    #region Methods

    private IEnumerator WaveControl()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            Debug.Log("Wave activated");
            waves[i].SpawnWave();
            gameManager.EnemiesAlive += waves[i].enemies.Count;
            if (waves[i].hasWaveTimer)
            {
                float timer = 0;
                while (!canSpawnNewWave)
                {
                    timer += Time.deltaTime;
                    if (timer >= waves[i].timer || gameManager.ControlIfSpawnedEnemiesAreDead())
                    {
                        canSpawnNewWave = true;
                    }
                    yield return null;
                }
            }
            else
            {
                yield return new WaitUntil(gameManager.ControlIfSpawnedEnemiesAreDead);
            }
            
            yield return new WaitUntil( () => canSpawnNewWave);
            canSpawnNewWave = false;
        }
        yield return null;
    }
    
    #endregion
}
