using UnityEngine;
public enum EnemyType
{
    Basic,
    Armored
}
public class EnemyPoolManager : ObjectPoolManagerBase<Enemy, EnemyType>
{
    private int activeEnemyCount = 0;
    public static new EnemyPoolManager Instance
    {
        get
        {
            return Singleton<EnemyPoolManager>.Instance;
        }
    }

    public int GetActiveEnemyCount()
    {
        return activeEnemyCount;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override Enemy Get(EnemyType type)
    {
        var enemy = base.Get(type);
        enemy.OnDie += HandleEnemyDeath;
        activeEnemyCount++;
        return enemy;
    }

    public override void Return(EnemyType type, Enemy enemy)
    {
        base.Return(type, enemy);
        enemy.OnDie -= HandleEnemyDeath;
        activeEnemyCount--;
    }

    private void HandleEnemyDeath()
    {
        activeEnemyCount--;
    }
}