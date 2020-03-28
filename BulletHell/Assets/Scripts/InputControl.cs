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

    [Title("References")]
    [SerializeField] private InputData inputData;
    [SerializeField] private Agent agent;
    [ShowIf("type", AgentType.Player)] 
    [SerializeField] private Camera cam;
    [SerializeField] private Movement movement;
    public Transform weapons;
    
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
        SetWeaponsTargetTag();
        
        if (!inputData)
        {
            inputData = ScriptableObject.CreateInstance<InputData>();
        }
        if (!cam && type == AgentType.Player)
        {
            cam = Camera.current;
        }
        if (!agent)
        {
            agent = GetComponent<Agent>();
        }
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        float speed = agent.basicSpeed + agent.extraSpeed;
        movement.Move(direction, speed);
    }

    private void OnDrawGizmosSelected()
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
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = ray.GetPoint(Vector3.Distance(cam.transform.position, transform.position));
            movement.LookAt(new Vector3(pos.x, transform.position.y, pos.z));

            // Vector2 viewportPosition = (Vector2)cam.ScreenToViewportPoint(Input.mousePosition) - new Vector2(.5f, .5f);
            // Vector3 mousePosition = new Vector3(viewportPosition.x, 0, viewportPosition.y);
            // movement.LookAt(transform.position + mousePosition);

            if (Input.GetKey(inputData.FirstWeapon) && agentCanShoot)
            {
                foreach (Transform weapon in weapons)
                {
                    weapon.GetComponent<WeaponControl>().ControlWeapon(agent.basicDamage + agent.extraDamage);
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

            Vector3 pointToLook = Vector3.zero;
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
            movement.LookAt(pointToLook);
            
            if (agentCanShoot)
            {
                foreach (Transform weapon in weapons)
                {
                    weapon.GetComponent<WeaponControl>().ControlWeapon(agent.basicDamage + agent.extraDamage);
                }
            }
        }

        #endregion
        
    }

    private void SetWeaponsTargetTag()
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
