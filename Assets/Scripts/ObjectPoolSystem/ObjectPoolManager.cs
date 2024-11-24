using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class ObjectPoolManagerBase<T, TEnum> : Singleton<ObjectPoolManagerBase<T, TEnum>>
    where T : MonoBehaviour, IPoolable
    where TEnum : System.Enum
{
    [System.Serializable]
    protected struct PoolEntry
    {
        public TEnum type;
        public T prefab;
    }

    [SerializeField] private List<PoolEntry> prefabs;
    [SerializeField] private int initialPoolSize = 10;

    private Dictionary<TEnum, ObjectPool<T>> pools;

    protected override void Awake()
    {
        base.Awake(); 
        InitializePools();
    }

    private void InitializePools()
    {
        pools = new Dictionary<TEnum, ObjectPool<T>>();

        foreach (var entry in prefabs)
        {
            if (!pools.ContainsKey(entry.type))
            {
                pools[entry.type] = new ObjectPool<T>(entry.prefab, initialPoolSize, transform);
            }
        }
    }

    public virtual T Get(TEnum type)
    {
        if (pools.TryGetValue(type, out var pool))
        {
            return pool.Get();
        }

        Debug.LogError($"Type {type} not defined in the pool.");
        return null;
    }
    public virtual T GetRandom()
    {
        if (pools.Count == 0)
        {
            Debug.LogError("No pools available to get a random object.");
            return null;
        }

        var randomIndex = UnityEngine.Random.Range(0, pools.Count);
        var randomType = (TEnum)(object)Enum.GetValues(typeof(TEnum)).GetValue(randomIndex);
        return Get(randomType);
    }
    public virtual void Return(TEnum type, T obj)
    {
        if (pools.TryGetValue(type, out var pool))
        {
            pool.Return(obj);
        }
        else
        {
            Debug.LogError($"Type {type} not defined in the pool.");
        }
    }
    public virtual void ReturnAll(TEnum type)
    {
        if (pools.TryGetValue(type, out var pool))
        {
            var activeObjects = pool.GetActiveObjects();
            foreach (var obj in activeObjects)
            {
                pool.Return(obj);
            }
        }
        else
        {
            Debug.LogError($"Type {type} not defined in the pool.");
        }
    }
}
