using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    #region Fields

    public enum AgentType
    {
        Player,
        Enemy
    }
    [EnumToggleButtons]
    public AgentType type;
    public bool agentCanShoot = true;

    [SerializeField] private InputData inputData;
    [SerializeField, HideIf("type", AgentType.Enemy)] private Camera cam;
    [SerializeField] private Movement movement;
    [SerializeField] private Transform weapons;
    
    
    [TitleGroup("AI"), ShowIf("type", AgentType.Enemy)]
    [SerializeField] private Transform target;
    [PropertySpace(10)]
    private enum MovementType {FollowTarget, Patrol}
    [TitleGroup("AI"), ShowIf("type", AgentType.Enemy)]
    [SerializeField] private MovementType movementType;
    [TitleGroup("AI"), ShowIf("type", AgentType.Enemy)]
    public List<Vector2> points = new List<Vector2>();
    [PropertySpace(10)]
    private enum RotationType {LookAtTarget, RotateItself, RotationPatrol}
    [TitleGroup("AI"), ShowIf("type", AgentType.Enemy)]
    [SerializeField] private RotationType rotationType;
    [TitleGroup("AI"), ShowIf("type", AgentType.Enemy)] 
    [SerializeField] private float rotationSpeed = 10f;
    
    private Vector2 direction = Vector2.zero;
    private int pointIndex = 0;
    private float currentAngle = 0;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        SetTargetTag();
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 point = new Vector3(points[i].x, 1, points[i].y);
            Gizmos.DrawSphere(point, 0.3f);
        }
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

            if (Input.GetKey(inputData.FirstWeapon) && agentCanShoot)
            {
                foreach (Transform weapon in weapons)
                {
                    weapon.GetComponent<WeaponControl>().ControlWeapon();
                }
            }
        }

        #endregion

        #region AI

        if (type == AgentType.Enemy)
        {
            Vector3 targetPoint = Vector3.zero;
            switch (movementType)
            {
                case MovementType.FollowTarget:
                    if (target != null)
                    {
                        targetPoint = target.position - transform.position;
                    }
                    break;
                
                case MovementType.Patrol:
                    Vector3 point = new Vector3(points[pointIndex].x, transform.position.y, points[pointIndex].y);
                    targetPoint = point - transform.position;
                    if (Vector3.Distance(transform.position, point) < .5f)
                    {
                        pointIndex = pointIndex >= points.Count-1 ? 0 : pointIndex+1;
                    }
                    break;
            }
            direction = new Vector2(targetPoint.x, targetPoint.z);

            Vector3 pointToLook = transform.position + transform.forward;
            switch (rotationType)
            {
                case RotationType.LookAtTarget:
                    if (target != null)
                    {
                        pointToLook = new Vector3(target.position.x, transform.position.y, target.position.z);
                    }
                    break;
                
                case RotationType.RotateItself:
                    currentAngle += rotationSpeed * Time.deltaTime;
                    if (currentAngle > 360f)
                    {
                        currentAngle = 0;
                    }
                    float x = transform.position.x + 1 * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
                    float z = transform.position.z + 1 * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
                    pointToLook = new Vector3(x, transform.position.y, z);
                    break;
                
                case RotationType.RotationPatrol:

                    break;
            }
            Debug.Log(pointToLook);
            movement.LookAt(pointToLook);
            
            if (agentCanShoot)
            {
                foreach (Transform weapon in weapons)
                {
                    weapon.GetComponent<WeaponControl>().ControlWeapon();
                }
            }
        }

        #endregion
        
    }

    private void SetTargetTag()
    {
        foreach (Transform weapon in weapons)
        {
            switch (type)
            {
                case AgentType.Player:
                    weapon.GetComponent<WeaponControl>().targetTag = AgentType.Enemy.ToString();
                    break;
                
                case AgentType.Enemy:
                    weapon.GetComponent<WeaponControl>().targetTag = AgentType.Player.ToString();
                    break;
            }
        }
    }

    #endregion
}
