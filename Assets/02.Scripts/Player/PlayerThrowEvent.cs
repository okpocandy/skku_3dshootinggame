using UnityEngine;

public class PlayerThrowEvent : MonoBehaviour
{
    public Player MyPlayer;
    public Melee MyMelee;
    public Weapon_Bomb MyBomb;

    public void ThrowEvent()
    {
        MyBomb.ThrowBomb();
    }

    public void MeleeEvent()
    {
        MyMelee.MeleeAttack();
    }
}
