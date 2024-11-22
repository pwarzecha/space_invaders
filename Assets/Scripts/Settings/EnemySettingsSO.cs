using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "Settings/EnemySettings")]
public class EnemySettingsSO : ScriptableObject
{
    [Header("General")]
    public EnemyType enemyType;
    public int scoreReward;

    [Header("Movement")]
    public float movementSpeed = 2.0f;

    [Header("Combat")]
    public int initHealth = 2;
    public float fireInterval = 2.5f;
    public float projectileSpeedBoost = 0f;
    public int projectileDamageBoost = 0;

    [Header("Power-Up Drop")]
    public float powerUpDropChance = 0.1f;
}
