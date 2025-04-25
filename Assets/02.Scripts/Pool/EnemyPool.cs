using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : GenericPool<Enemy>
{
    protected override void InitializePool()
    {
        base.InitializePool();
        ShuffleQueue();
    }

    private void ShuffleQueue()
    {
        var tempList = new List<Enemy>(objectPool.Count);
        while (objectPool.Count > 0)
        {
            tempList.Add(objectPool.Dequeue());
        }

        // Fisher-Yates shuffle
        for (int i = tempList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = tempList[i];
            tempList[i] = tempList[j];
            tempList[j] = temp;
        }

        foreach (var enemy in tempList)
        {
            objectPool.Enqueue(enemy);
        }
    }
}
