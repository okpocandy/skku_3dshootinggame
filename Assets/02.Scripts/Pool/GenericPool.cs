using System.Collections.Generic;
using UnityEngine;

public class GenericPool<T> : Singleton<GenericPool<T>> where T : Component
{
    [SerializeField] protected List<T> prefabs;
    [SerializeField] protected int poolSize = 50;
    
    protected Queue<T> objectPool = new Queue<T>();
    protected List<T> activeObjects = new List<T>();

    private void Awake()
    {
        InitializePool();
    }

    protected virtual void InitializePool()
    {
        foreach (T prefab in prefabs)
        {
            for (int i = 0; i < poolSize; i++)
            {
                T obj = Instantiate(prefab, transform);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }
    }

    public T GetObject()
    {
        if (objectPool.Count > 0)
        {
            T obj = objectPool.Dequeue();
            obj.gameObject.SetActive(true);
            activeObjects.Add(obj);
            return obj;
        }
        else
        {
            // 채워 넣기
            InitializePool();
            return GetObject();
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
        activeObjects.Remove(obj);
    }

    public void ReturnAllObjects()
    {
        foreach (var obj in activeObjects.ToArray())
        {
            ReturnObject(obj);
        }
    }
} 