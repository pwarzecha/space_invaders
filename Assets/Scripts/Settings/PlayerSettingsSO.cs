using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5.0f;
    public float tiltAngle = 30f; 
    public float tiltSpeed = 10f;

    [Header("Combat")]
    public int maxHealth = 3;

    public float baseFireInterval = 0.5f;
    public float minFireInterval = 0.05f;

    public int baseDamage = 1;
    public int maxDamage = 5;

    public int baseProjectilesAmount = 1;
    public int maxProjectilesAmount = 5;

    [Header("Others")]
    public Vector3 projectileSpawnOffset;

}
