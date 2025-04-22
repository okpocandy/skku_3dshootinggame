using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : Singleton<WeaponPool>
{
    [SerializeField]
    private GameObject _bombPrefab;
    private List<Bomb> _bombPool = new List<Bomb>();
    private int _bombCount = 3;
    private void Awake()
    {
        InitializePool(_bombCount);
    }

    private void InitializePool(int count)
    {
        for(int i=0; i<count; i++)
        {
            _bombPool.Add(CreateBomb());
        }
    }

    private Bomb CreateBomb()
    {
        Bomb bomb = Instantiate(_bombPrefab).GetComponent<Bomb>();
        bomb.gameObject.SetActive(false);
        bomb.transform.SetParent(transform);
        return bomb;
    }

    public Bomb GetBomb()
    {
        foreach(Bomb bomb in _bombPool)
        {
            if(bomb.gameObject.activeInHierarchy == false)
            {
                bomb.gameObject.SetActive(true);
                return bomb;
            }
        }
        return null;

    }
}
