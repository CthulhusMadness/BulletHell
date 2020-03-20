using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Fields

    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform graphics;

    #endregion

    #region UnityCallbacks



    #endregion

    #region Methods

    public virtual void Move(Vector2 direction)
    {
        float step = speed * Time.deltaTime;
        Vector3 movement = Vector3.zero;
        if (direction != Vector2.zero)
        {
            movement = new Vector3(direction.x, 0, direction.y).normalized;
        }
        
        transform.Translate(movement * step);
    }

    public void Rotate()
    {
        
    }

    public void LookAt(Vector3 target)
    {
        graphics.LookAt(target);
    }

    #endregion
}
