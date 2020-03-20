using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Fields

    [SerializeField] private InputData inputData;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform graphics;
    [SerializeField] private Transform weapon;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float shotDelay = .5f;
    [SerializeField] private float projectileSpeed = 1f;

    private float horiz = 0;
    private float vert = 0;
    private float timer = 0;
    private bool isShooting = false;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        timer = 0;
        
        if (inputData == null)
        {
            inputData = ScriptableObject.CreateInstance<InputData>();
        }

        if (cam == null)
        {
            cam = Camera.current;
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move(horiz, vert);
    }

    #endregion

    #region Methods

    private void GetInput()
    {
        // Keyboard
        horiz = Convert.ToInt32(Input.GetKey(inputData.Right)) - Convert.ToInt32(Input.GetKey(inputData.Left));
        vert = Convert.ToInt32(Input.GetKey(inputData.Up)) - Convert.ToInt32(Input.GetKey(inputData.Down));
        
        
        // mouse
        Vector2 viewportPosition = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f);
        Vector3 mousePosition = new Vector3(viewportPosition.x, 0, viewportPosition.y);
        graphics.LookAt(transform.position + mousePosition);

        if (Input.GetKey(inputData.FirstWeapon))
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Shoot();
                timer = shotDelay;
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void Shoot()
    {
        GameObject instance = Instantiate(projectilePrefab, weapon.position, Quaternion.identity);
        float step = projectileSpeed;
        instance.GetComponent<Rigidbody>().AddForce(weapon.forward * step, ForceMode.VelocityChange);
    }

    private void Move(float horizontal, float vertical)
    {
        float step = speed * Time.deltaTime;
        Vector3 movement = Vector3.zero;
        if (horizontal != 0 || vertical != 0)
        {
            movement = new Vector3(horizontal, 0, vertical).normalized;
        }
        
        transform.Translate(movement * step);
    }
    
    #endregion
}
