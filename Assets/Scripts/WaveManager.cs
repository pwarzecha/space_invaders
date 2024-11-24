using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks; 
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveDataSO> _waves;
    [SerializeField] private GameDataSO _gameDataSO;

    private int _currentWaveIndex = 0;
    private bool _stopWaves = false;
    private List<Enemy> _currentFormationEnemies = new List<Enemy>();

    public Action<int> OnUpdateScoreRequest;
    public async UniTask StartHandlingWaves()
    {
        foreach (var wave in _waves)
        {
            if (_stopWaves) break;

            _currentWaveIndex++;
            Debug.Log($"Starting Wave {_currentWaveIndex}");

            PopupManager.Instance.ShowPopup(PopupType.WaveStart);
            await UniTask.Delay((int)(wave.preparationTime * 1000));

            PopupManager.Instance.ShowPopup(PopupType.WarmupPhase);
            await RandomSpawnPhase(wave);

            PopupManager.Instance.ShowPopup(PopupType.FormationPhase);
            await FormationPhase(wave);
        }
    }

    public void StopWaves()
    {
        _stopWaves = true;
    }

    private async UniTask RandomSpawnPhase(WaveDataSO wave)
    {
        Debug.Log("Random Spawn Phase Started");
        float elapsedTime = 0f;

        while (elapsedTime < wave.phaseTime && !_stopWaves)
        {
            SpawnRandomEnemy();
            await UniTask.Delay((int)(wave.randomSpawnInterval * 1000));
            elapsedTime += wave.randomSpawnInterval;
        }

        Debug.Log("Random Spawn Phase Ended");
    }

    private void SpawnRandomEnemy()
    {
        EnemyType randomType = _gameDataSO.GetRandomEnemyType();
        var enemy = EnemyPoolManager.Instance.Get(randomType);

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        enemy.transform.position = new Vector3(
            UnityEngine.Random.Range(screenMin.x, screenMax.x),
            screenMax.y,
            0
        );

        enemy.OnUpdateScoreRequest += UpdateScore;
    }

    private void UpdateScore(int scoreToAdd)
    {
        OnUpdateScoreRequest?.Invoke(scoreToAdd);
    }
    private void OnRemovedFromMap(Enemy enemy)
    {
        enemy.OnRemovedFromMap -= OnRemovedFromMap;
        _currentFormationEnemies.Remove(enemy);
    }
    private async UniTask FormationPhase(WaveDataSO wave)
    {
        Debug.Log("Formation Phase Started");
        _currentFormationEnemies.Clear();

        foreach (var formation in wave.formations)
        {
            var enemy = SpawnFormationEnemy(formation);
            enemy.OnUpdateScoreRequest += UpdateScore;
            enemy.OnRemovedFromMap += OnRemovedFromMap;
            _currentFormationEnemies.Add(enemy);
        }

        while (_currentFormationEnemies.Count > 0 && !_stopWaves)
        {
            MoveFormation();
            await UniTask.Yield();
        }

        Debug.Log("Formation Phase Ended");
    }

    private Enemy SpawnFormationEnemy(FormationData formation)
    {
        var enemy = EnemyPoolManager.Instance.Get(formation.enemyType);
        enemy.transform.position = formation.spawnPosition;
        enemy.SetFormationMovement(Vector3.left, formation.speed);
        return enemy;
    }

    private void MoveFormation()
    {
        if (_currentFormationEnemies.Count == 0) return;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (var enemy in _currentFormationEnemies)
        {
            if (enemy == null || !enemy.gameObject.activeSelf) continue;

            Vector3 position = enemy.transform.position;
            minX = Mathf.Min(minX, position.x);
            maxX = Mathf.Max(maxX, position.x);
        }

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        if (minX <= screenMin.x || maxX >= screenMax.x)
        {
            foreach (var enemy in _currentFormationEnemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    enemy.InvertDirection();
                }
            }
        }

        foreach (var enemy in _currentFormationEnemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                enemy.MoveWithFormation();
            }
        }
    }
}