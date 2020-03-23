using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputData", menuName = "Input")]
public class InputData : ScriptableObject
{
    #region Fields

    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode FirstWeapon = KeyCode.Mouse0;
    public KeyCode SecondaryWeapon = KeyCode.Mouse1;

    #endregion

    #region UnityCallbacks



    #endregion

    #region Methods



    #endregion
}
