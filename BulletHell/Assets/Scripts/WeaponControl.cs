using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    #region Fields

    public bool canShoot;
    
    [NonSerialized] public string targetTag;

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject projectilePrefab;
    
    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        canShoot = true;
    }

    #endregion

    #region Methods

    public void ControlWeapon()
    {
        if (canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        StartCoroutine(weaponData.Shoot(transform, this, targetTag));
    }

    #endregion
}
