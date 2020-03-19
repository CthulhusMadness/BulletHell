using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Fields

    [SerializeField] private InputData inputData;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform graphics = null;
    [SerializeField] private float speed = 5f;
    

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        Debug.Log(inputData);
        if (inputData == null)
        {
            inputData = ScriptableObject.CreateInstance<InputData>();
        }

        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void FixedUpdate()
    {
        GetInput();
    }


    #endregion

    #region Methods

    private void GetInput()
    {
        // Keyboard
        float horizontal = Convert.ToInt32(Input.GetKey(inputData.Right)) - Convert.ToInt32(Input.GetKey(inputData.Left));
        float vertical = Convert.ToInt32(Input.GetKey(inputData.Up)) - Convert.ToInt32(Input.GetKey(inputData.Down));
        float step = speed * Time.deltaTime;
        Vector3 movement = Vector3.zero;
        if (horizontal != 0 || vertical != 0)
        {
            movement = new Vector3(horizontal, 0, vertical).normalized;
        }
        
        transform.Translate(movement * step);
        
        // mouse
        Vector2 viewportPosition = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f);
        Vector3 mousePosition = new Vector3(viewportPosition.x, 0, viewportPosition.y);
        graphics.LookAt(transform.position + mousePosition);
    }
    
    #endregion
}
