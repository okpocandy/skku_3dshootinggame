using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponTypes
{
    Gun,
    Melee,
    Bomb
}

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> Weapons;
    public List<Sprite> WeaponIcons;
    public List<Sprite> Crosshairs;

    [SerializeField]
    private WeaponTypes _currentWeaponType;
    [SerializeField]
    private GameObject _currentWeapon;

    public Image WeaponIcon;
    public Image Crosshair;

    public int CurrentWeaponIndex = 0;

    private void Start()
    {
        _currentWeaponType = WeaponTypes.Gun;
        _currentWeapon = Weapons[(int)_currentWeaponType];
        _currentWeapon.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(0);
            CurrentWeaponIndex = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(1);
            CurrentWeaponIndex = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(2);
            CurrentWeaponIndex = 2;
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            CurrentWeaponIndex++;
            if(CurrentWeaponIndex >= Weapons.Count)
            {
                CurrentWeaponIndex = 0;
            }
            SetWeapon(CurrentWeaponIndex);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            CurrentWeaponIndex--;
            if(CurrentWeaponIndex < 0)
            {
                CurrentWeaponIndex = Weapons.Count - 1;
            }
            SetWeapon(CurrentWeaponIndex);
        }
    }

    private void SetWeapon(int weaponIndex)
    {
        _currentWeapon.SetActive(false);
        _currentWeaponType = (WeaponTypes)weaponIndex;
        _currentWeapon = Weapons[(int)_currentWeaponType];
        WeaponIcon.sprite = WeaponIcons[weaponIndex];
        Crosshair.sprite = Crosshairs[weaponIndex];
        _currentWeapon.SetActive(true);
    }
} 