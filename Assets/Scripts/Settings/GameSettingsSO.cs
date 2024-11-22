using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("General")]
    public int initialPlayerHealth;

    [Header("Player")]
    [SerializeField] private Vector3 _spawnPosition;

    [Header("Enemy")]
    public float enemySpawnInterval = 1.0f;
    public Vector3 spawnAreaMin = new Vector3(-5, 5, 0);
    public Vector3 spawnAreaMax = new Vector3(5, 5, 0);

    [Header("Power-Up")]
    public float powerUpSpawnChance = 0.1f;
}
