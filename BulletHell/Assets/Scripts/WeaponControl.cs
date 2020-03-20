using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private float shotDelay = .5f;
    [SerializeField] private bool startWithDelay = false;
    [SerializeField, ShowIf("startWithDelay")]
    private float startDelay = .25f;
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private GameObject projectilePrefab;
    
    private float timer = 0;
    
    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        if (startWithDelay)
        {
            SetTimer(startDelay);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    #endregion

    #region Methods

    public void ControlWeapon()
    {
        if (timer <= 0)
        {
            Shoot();
            timer = shotDelay;
        }
    }

    private void Shoot()
    {
        GameObject instance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        float step = projectileSpeed;
        instance.GetComponent<Rigidbody>().AddForce(transform.forward * step, ForceMode.VelocityChange);
    }

    public void SetTimer(float t)
    {
        timer = t;
    }

    #endregion
}
