﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    #region Fields

    [PropertyRange(1, 50)] public int quantity = 1;
    public float fireRate = 0.5f;
    public float waveDelay = 0f;
    public float angle = 0;

    #endregion

    #region UnityCallbacks

    #endregion

    #region Methods

    public IEnumerator Shoot(Transform weaponPoint, GameObject projectilePrefab, WeaponControl weaponControl,
        string targetTag)
    {
        weaponControl.canShoot = false;
        for (int i = 0; i < quantity;)
        {
            GameObject instance = Instantiate(projectilePrefab, weaponPoint.position, weaponPoint.rotation);
            ProjectileControl projectileControl = instance.GetComponent<ProjectileControl>();
            projectileControl.targetTag = targetTag;
            var rot = Quaternion.AngleAxis(angle * i - angle * ((quantity - 1) / 2f), Vector3.up);
            var lDirection = rot * weaponPoint.forward;
            projectileControl.PushToDirection(lDirection);
            if (fireRate > 0f)
            {
                yield return new WaitForSeconds(fireRate);
            }

            i++;
        }

        yield return new WaitForSeconds(waveDelay);
        weaponControl.canShoot = true;
    }

    #endregion
}