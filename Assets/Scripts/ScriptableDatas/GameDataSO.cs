using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public EnemyType type;
    public float spawnChance;
}

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameDataSO : ScriptableObject
{
    [Header("Player")]
    public Vector3 spawnPosition;

    [Header("Enemy")]
    public float enemySpawnInterval = 1.0f;
    public List<EnemySpawnData> enemySpawnChances;

    [Header("Camera Shake")]
    public float cameraShakeStrength = 0.5f;
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
