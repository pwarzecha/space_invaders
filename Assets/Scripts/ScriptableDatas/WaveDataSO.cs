using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveDataSO : ScriptableObject
{
    [Header("Random Spawn Phase")]
    public float preparationTime = 2f; 
    public float phaseTime = 10f; 
    public float randomSpawnInterval = 1f; 

    [Header("Formation Phase")]
    public List<FormationData> formations; 
}

[System.Serializable]
public struct FormationData
{
    public EnemyType enemyType; 
    public Vector3 spawnPosition; 
    public float speed; 
}
