using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettings", menuName = "Settings/ProjectileSettings")]
public class ProjectileSettingsSO : ScriptableObject
{
    public float baseSpeed = 5.0f;
    public int baseDamage = 1;
}

