using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly List<T> activeObjects = new List<T>();
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
        activeObjects.Add(obj);
        return obj;
    }

    public void Return(T obj)
    {
        if (activeObjects.Remove(obj))
        {
            obj.OnReturn();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    public List<T> GetActiveObjects()
    {
        return new List<T>(activeObjects);
    }
}
