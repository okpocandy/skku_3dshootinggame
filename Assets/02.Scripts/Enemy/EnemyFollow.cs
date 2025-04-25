using UnityEngine;

public class EnemyFollow : Enemy
{
    protected override void Start()
    {
        base.Start();

        FindDistance = 999999f;
        ReturnDistance = 999999f;
        CurrentState = EnemyState.Trace;
    }

    private void Update()
    {
        switch(CurrentState)
        {
            case EnemyState.Trace:
                Trace();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                // Damaged 상태일 때는 base 클래스의 Damaged_Coroutine이 실행되도록 함
                break;
            case EnemyState.Die:
                break;
        }
    }
}
