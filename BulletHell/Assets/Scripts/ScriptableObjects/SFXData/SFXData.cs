using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "SFX/Data")]
public class SFXData : ScriptableObject
{
    public AudioClip clip;
    [MinValue(0f)]
    public float volume;
    [MinValue(0f)]
    public float pitch;
}
