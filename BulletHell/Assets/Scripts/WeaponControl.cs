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
    public WeaponData temporaryWeapon;
    [SerializeField] private GameObject projectilePrefab;
    
    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        canShoot = true;
    }

    #endregion

    #region Methods

    public void ControlWeapon(int damage)
    {
        if (canShoot)
        {
            Shoot(damage);
        }
    }

    private void Shoot(int damage)
    {
        if (weaponData && ObjectPooler.Instance)
        {
            if (temporaryWeapon)
            {
                StartCoroutine(temporaryWeapon.Shoot(transform, this, targetTag, damage));
            }
            else
            {
                StartCoroutine(weaponData.Shoot(transform, this, targetTag, damage));
            }
        }
    }

    #endregion
}
