using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    #region Fields

    [SerializeField] private int HP = 5;
    [SerializeField] private InputControl input;
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject deathParticle;

    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        if (!input)
        {
            input = GetComponent<InputControl>();
        }
    }

    #endregion

    #region Methods

    public void Hit()
    {
        HP -= 1;
        if (input && HP <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        switch (input.type)
        {
            case InputControl.AgentType.Player:
                Camera.main.transform.SetParent(null);
                GameManager.Instance.PlayerDeath();
                Instantiate(deathParticle, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
            
            case InputControl.AgentType.Enemy:
                GameManager.Instance.EnemiesAlive -= 1;
                gameObject.SetActive(false);
                break;
        }
    }

    #endregion
}
