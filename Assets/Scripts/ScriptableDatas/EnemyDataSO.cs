using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("General")]
    public EnemyType enemyType;
    public int scoreReward;

    [Header("Movement")]
    public float movementSpeed = 2.0f;

    [Header("Combat")]
    public int initHealth = 2;
    public float fireInterval = 2.5f;
    public int _projectileDamage = 1;

    [Header("Power-Up Drop")]
    public float powerUpDropChance = 0.1f;
}
