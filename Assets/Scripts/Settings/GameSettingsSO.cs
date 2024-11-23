using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public EnemyType type;
    public float spawnChance;
}

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("General")]
    public Vector2 screenBoundsX;
    public Vector2 screenBoundsY;

    [Header("Player")]
    public Vector3 spawnPosition;

    [Header("Enemy")]
    public float enemySpawnInterval = 1.0f;
    public Vector3 spawnAreaMin = new Vector3(-5, 5, 0);
    public Vector3 spawnAreaMax = new Vector3(5, 5, 0);
    public List<EnemySpawnData> enemySpawnChances;

    public EnemyType GetRandomEnemyType()
    {
        float totalWeight = 0;
        foreach (var data in enemySpawnChances)
        {
            totalWeight += data.spawnChance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (var data in enemySpawnChances)
        {
            cumulativeWeight += data.spawnChance;
            if (randomValue <= cumulativeWeight)
            {
                return data.type;
            }
        }

        return EnemyType.Basic;
    }
}
