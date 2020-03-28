using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    #region Fields

    public float basicSpeed = 8f;
    public float extraSpeed = 0f;
    public int basicDamage = 1;
    public int extraDamage = 0;
    [SerializeField] private int HP = 5;

    [NonSerialized] public PowerUpData currentPowerUp;
    
    public InputControl input;
    [SerializeField] private ParticleSystem damageParticle;
    [SerializeField] private GameObject deathParticle;

    private IEnumerator coroutine;

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

    public void Hit(int damage)
    {
        HP -= damage;

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

    public void ActivatePowerUpTimer()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = PowerUpTimer();
        StartCoroutine(coroutine);
    }
    
    private IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(currentPowerUp.timer);
        DeletePowerUp();
    }

    public void DeletePowerUp()
    {
        currentPowerUp.DeletePowerUp(this);
    }

    #endregion
}
