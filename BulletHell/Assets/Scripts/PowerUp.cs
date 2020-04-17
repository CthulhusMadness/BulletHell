using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Fields

    [SerializeField] private PowerUpData powerUpData;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float floatingForce;
    [SerializeField] private float rotationSpeed;

    private const string targetTag = "Player";

    private Vector3 startingPoint;

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

    private void Start()
    {
        startingPoint = transform.position;
    }

    private void Update()
    {
        Floating();
    }

    #endregion

    #region Methods

    private void Floating()
    {
        float newPosY = startingPoint.y + Mathf.Sin(Time.time * floatingSpeed) * floatingForce;
        transform.position = new Vector3(startingPoint.x, newPosY, startingPoint.z);
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        float newRotY = eulerAngles.y + Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(eulerAngles.x, newRotY, eulerAngles.z);
    }

    #endregion
}
