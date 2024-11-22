using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings")]
public class PlayerSettingsSO : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 5.0f;

    [Header("Combat")]
    public int maxHealth = 3;
    public float fireInterval = 0.4f;
    public float baseProjectileSpeedBoost = 0f;
    public int baseProjectileDamageBoost = 0;
    public int baseProjectilesAmount = 1;
    public int maxProjectilesAmount = 5;

}
