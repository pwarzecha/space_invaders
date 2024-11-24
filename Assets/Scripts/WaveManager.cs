using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveDataSO> waves; 
    [SerializeField] private GameDataSO gameSettingsSO;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;

    public void StartWaves()
    {
        StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        foreach (var wave in waves)
        {
            currentWaveIndex++;
            Debug.Log($"Starting Wave {currentWaveIndex}");

            yield return StartCoroutine(RandomSpawnPhase(wave));
            yield return StartCoroutine(FormationPhase(wave));
        }

        Debug.Log("All waves completed!");
    }

    private IEnumerator RandomSpawnPhase(WaveDataSO wave)
    {
        Debug.Log("Random Spawn Phase Started");
        float elapsedTime = 0f;

        while (elapsedTime < wave.randomSpawnDuration)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(wave.randomSpawnInterval);
            elapsedTime += wave.randomSpawnInterval;
        }

        Debug.Log("Random Spawn Phase Ended");
    }

    private void SpawnRandomEnemy()
    {
        EnemyType randomType = gameSettingsSO.GetRandomEnemyType();
        var enemy = EnemyPoolManager.Instance.Get(randomType);

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        enemy.transform.position = new Vector3(
            Random.Range(screenMin.x, screenMax.x),
            screenMax.y,
            0
        );
    }

    private IEnumerator FormationPhase(WaveDataSO wave)
    {
        Debug.Log("Formation Phase Started");

        foreach (var formation in wave.formations)
        {
            SpawnFormationEnemy(formation);
            yield return new WaitForSeconds(0.5f); 
        }

        Debug.Log("Formation Phase Ended");
        yield return new WaitUntil(() => EnemyPoolManager.Instance.GetActiveEnemyCount() == 0);
    }

    private void SpawnFormationEnemy(FormationData formation)
    {
        var enemy = EnemyPoolManager.Instance.Get(formation.enemyType);
        enemy.transform.position = formation.spawnPosition;
        enemy.SetFormationMovement(formation.moveDirection, formation.speed);
    }
}
