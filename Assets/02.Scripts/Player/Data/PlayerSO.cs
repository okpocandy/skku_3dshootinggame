using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public float MoveSpeed = 7f;
    public float RunSpeed = 12f;
    public float JumpPower = 5f;
    public float SlideSpeed = 20f;

    public float MaxStamina = 100f;
    public float AddStamina = 20f;
    public float SubStamina = 10f;
    public float MinRunStamina = 0.5f;

    public float SlidingStamina = 25f;
    public float SlideTime = 0.3f;

    public float ClimbSpeed = 3f;
}
