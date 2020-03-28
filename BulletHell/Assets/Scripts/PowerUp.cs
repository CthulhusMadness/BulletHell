using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Fields

    [SerializeField] private PowerUpData powerUpData;

    private const string targetTag = "Player";

    #endregion

    #region UnityCallbacks

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Apply powerUp");
            powerUpData.ApplyPowerUp(other.GetComponent<Agent>());
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods

    

    #endregion
}
