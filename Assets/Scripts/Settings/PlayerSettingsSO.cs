using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 5.0f;

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
