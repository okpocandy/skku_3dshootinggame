using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Singleton<ItemSpawner>
{
    public Enemy Enemy;
    public int SpawnGoldCount = 20;
    public float SpawnRadius = 2f;


    public void SpawnGold(Vector3 position)
    {
        List<Item_GoldCoin> goldCoins = new List<Item_GoldCoin>();
        for(int i=0; i<SpawnGoldCount; i++)
        {
            Item_GoldCoin goldCoin = GoldPool.Instance.GetObject();
            goldCoins.Add(goldCoin);
        }

        foreach(var goldCoin in goldCoins)
        {
            Vector3 randomPosition = position + Random.insideUnitSphere * SpawnRadius;
            randomPosition.y = Mathf.Clamp(randomPosition.y, 0.2f, 0.6f);
            goldCoin.transform.position = randomPosition;
        }
    }
}
