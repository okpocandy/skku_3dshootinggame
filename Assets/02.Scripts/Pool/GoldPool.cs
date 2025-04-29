using UnityEngine;

public class GoldPool : GenericPool<Item_GoldCoin>
{
    [SerializeField]
    private int _poolSize = 400;
    // TrailPool specific functionality can be added here if needed
    override protected void InitializePool()
    {
        poolSize = _poolSize;
        base.InitializePool();
    }
}
