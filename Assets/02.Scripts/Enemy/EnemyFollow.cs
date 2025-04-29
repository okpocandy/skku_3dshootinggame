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
                _animator.SetTrigger("IdleToMove");
                Trace();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;
        }
    }
}
