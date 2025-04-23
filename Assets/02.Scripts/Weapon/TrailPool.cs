using System.Collections.Generic;
using UnityEngine;

public class TrailPool : Singleton<TrailPool>
{
    [SerializeField] private TrailRenderer trailPrefab;
    [SerializeField] private int poolSize = 55;
    
    private Queue<TrailRenderer> trailPool = new Queue<TrailRenderer>();
    private List<TrailRenderer> activeTrails = new List<TrailRenderer>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            TrailRenderer trail = Instantiate(trailPrefab, transform);
            trail.gameObject.SetActive(false);
            trailPool.Enqueue(trail);
        }
    }

    public TrailRenderer GetTrail()
    {
        if (trailPool.Count > 0)
        {
            TrailRenderer trail = trailPool.Dequeue();
            trail.gameObject.SetActive(true);
            activeTrails.Add(trail);
            return trail;
        }
        return null;
    }

    public void ReturnTrail(TrailRenderer trail)
    {
        trail.gameObject.SetActive(false);
        trailPool.Enqueue(trail);
        activeTrails.Remove(trail);
    }

    public void ReturnAllTrails()
    {
        foreach (var trail in activeTrails.ToArray())
        {
            ReturnTrail(trail);
        }
    }
} 