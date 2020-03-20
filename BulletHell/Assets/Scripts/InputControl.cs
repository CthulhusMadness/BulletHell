using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    #region Fields

    public enum AgentType
    {
        Player,
        AI
    }
    [EnumToggleButtons]
    public AgentType type;
    [HideIf("type", AgentType.Player)]
    public Transform target;
    public bool canShoot;

    [SerializeField] private InputData inputData;
    [SerializeField, HideIf("type", AgentType.AI)] private Camera cam;
    [SerializeField] private Movement movement;
    [SerializeField] private WeaponControl weaponControl;

    private Vector2 direction;
    private float timer = 0;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        timer = 0;
        
        if (inputData == null)
        {
            inputData = ScriptableObject.CreateInstance<InputData>();
        }

        if (cam == null && type == AgentType.Player)
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
        movement.Move(direction);
    }

    #endregion

    #region Methods

    private void GetInput()
    {
        #region Player

        if (type == AgentType.Player)
        {
            // Keyboard
            float horiz = Convert.ToInt32(Input.GetKey(inputData.Right)) - Convert.ToInt32(Input.GetKey(inputData.Left));
            float vert = Convert.ToInt32(Input.GetKey(inputData.Up)) - Convert.ToInt32(Input.GetKey(inputData.Down));
            direction = new Vector2(horiz, vert);

            // mouse
            Vector2 viewportPosition = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f);
            Vector3 mousePosition = new Vector3(viewportPosition.x, 0, viewportPosition.y);
            movement.LookAt(transform.position + mousePosition);

            if (Input.GetKey(inputData.FirstWeapon) && canShoot)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    weaponControl.Shoot();
                    timer = weaponControl.shotDelay;
                }
            }
            else
            {
                timer = 0;
            }
        }

        #endregion

        #region AI

        if (type == AgentType.AI)
        {
            if (target != null)
            {
                Vector3 targetPoint = target.position - transform.position;
                direction = new Vector2(targetPoint.x, targetPoint.z);
                
                Vector3 pointToLook = new Vector3(target.position.x, transform.position.y, target.position.z);
                movement.LookAt(pointToLook);
            }
            
            if (canShoot)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    weaponControl.Shoot();
                    timer = weaponControl.shotDelay;
                }
            }
            else
            {
                timer = 0;
            }
        }

        #endregion
        
    }

    #endregion
}
