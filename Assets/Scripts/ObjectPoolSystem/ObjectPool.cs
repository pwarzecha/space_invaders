using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private T CreateNewObject()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.OnCreated();
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
        return obj;
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            CreateNewObject();
        }

        var obj = _pool.Dequeue();
        obj.OnPooled();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.OnReturn();
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
