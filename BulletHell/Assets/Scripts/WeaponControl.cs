using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    #region Fields
    
    public float shotDelay = .5f;
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform weaponPoint;
    
    #endregion

    #region UnityCallbacks

    

    #endregion

    #region Methods

    public void Shoot()
    {
        GameObject instance = Instantiate(projectilePrefab, weaponPoint.position, Quaternion.identity);
        float step = projectileSpeed;
        instance.GetComponent<Rigidbody>().AddForce(weaponPoint.forward * step, ForceMode.VelocityChange);
    }

    #endregion
}
