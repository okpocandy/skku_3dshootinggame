using UnityEngine;
using System.Collections;

public class EnemyFollow : Enemy
{
    protected override void Start()
    {
        // 부모 클래스의 Start() 호출 전에 _player 초기화
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogWarning("Player not found! Retrying in next frame...");
            StartCoroutine(FindPlayerCoroutine());
            return;
        }

        base.Start();

        FindDistance = 999999f;
        ReturnDistance = 999999f;
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator FindPlayerCoroutine()
    {
        while (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null)
            {
                base.Start();
                FindDistance = 999999f;
                ReturnDistance = 999999f;
                CurrentState = EnemyState.Trace;
                break;
            }
            yield return null;
        }
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
