using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Weapons/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public Sprite weaponIcon;
    public Sprite crosshair;
    public WeaponTypes weaponType;
} 