using System.Collections.Generic;
using Cysharp.Threading.Tasks; // UniTask namespace
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveDataSO> waves;
    [SerializeField] private GameDataSO gameSettingsSO;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;

    public async UniTask StartHandlingWaves()
    {
        foreach (var wave in waves)
        {
            currentWaveIndex++;
            Debug.Log($"Starting Wave {currentWaveIndex}");

            await RandomSpawnPhase(wave);
            await FormationPhase(wave);
        }

        Debug.Log("All waves completed!");
    }

    private async UniTask RandomSpawnPhase(WaveDataSO wave)
    {
        Debug.Log("Random Spawn Phase Started");
        float elapsedTime = 0f;

        while (elapsedTime < wave.randomSpawnDuration)
        {
            SpawnRandomEnemy();
            await UniTask.Delay((int)(wave.randomSpawnInterval * 1000));
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

    private async UniTask FormationPhase(WaveDataSO wave)
    {
        Debug.Log("Formation Phase Started");

        foreach (var formation in wave.formations)
        {
            SpawnFormationEnemy(formation);
            await UniTask.Delay(500);
        }

        Debug.Log("Formation Phase Ended");

        await UniTask.WaitUntil(() => EnemyPoolManager.Instance.GetActiveEnemyCount() == 0);
    }

    private void SpawnFormationEnemy(FormationData formation)
    {
        var enemy = EnemyPoolManager.Instance.Get(formation.enemyType);
        enemy.transform.position = formation.spawnPosition;
        enemy.SetFormationMovement(formation.moveDirection, formation.speed);
    }
}
