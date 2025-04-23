using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 0: 대기 1: 추적 2: 복귀 3: 공격 4: 피격 5: 사망 6: 순찰
    public enum EnemyState
    {
        Idle,
        Trace,
        Return,
        Attack,
        Damaged,
        Die,
        Patrol,
    }
    public EnemyState CurrentState = EnemyState.Idle;

    
    public float FindDistance = 7f;      // 적 발견 범위
    public float ReturnDistance = 10f;   // 적 복귀 범위
    public float MoveSpeed = 3.3f;       // 적 이동 속도
    public float AttackDistance = 2.5f;  // 적 공격 인식 범위
    public float AttackCoolTime  = 2f;   // 적 공격 쿨타임
    public int Health = 100;
    public float DamagedTime = 0.5f;     // 피격 효과 지속 시간
    public float DieTime = 2f;
    public float PatrolTime = 5f;         // 패트롤 시간
    
    private float _damagedTimer = 0f;    // 피격 효과 타이머
    private float _attackTimer = 0f;     // 적 공격 쿨타임 타이머
    private float _patrolTimer = 0f;     // 패트롤 타이머

    private int _patrolIndex = 0;

    private GameObject _player;
    private CharacterController _characterController;

    private Vector3 _startPosition;
    [SerializeField]
    private List<Transform> _patrolPoints = new List<Transform>();


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
        _attackTimer = AttackCoolTime;
    }

    private void Update()
    {
        switch(CurrentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Trace:
                Trace();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
        }
    }

    public void TakeDamage(Damage damage)
    {
        if(CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;

        // 넉백
        Vector3 knockbackDirection = (transform.position - _player.transform.position).normalized;
        _characterController.Move(knockbackDirection * damage.From.GetComponent<Gun>().KnockbackForce);

        if(Health <= 0)
        {
            Debug.Log($"상태전환: {CurrentState} -> Die");
            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log($"상태전환: {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }

    // 상태와 똑같은 이름으로 메서드를 만들어준다.
    private void Idle()
    {
        // 전이 조건: 플레이어와 가까워 지면 -> 추적
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        _patrolTimer += Time.deltaTime;
        if(_patrolTimer >= PatrolTime)
        {
            Debug.Log("상태전환: Idle -> Patrol");
            CurrentState = EnemyState.Patrol;
            _patrolIndex = 0;
            return;
        }
        // 행동: 가만히 있는다.

    }
    private void Trace()
    {
        // 전이 조건: 플에이어와 멀어지면 -> Return
        if(Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }
        // 전이 조건 공격 범위 만큼 가까워지면 -> Attack
        if(Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 행동: 플레이어를 추적한다.
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _characterController.Move(direction * MoveSpeed * Time.deltaTime);
        

    }

    private void Return()
    {
        // 전이 조건: 시작 위치와 가까워 지면 -> Idle
        if(Vector3.Distance(transform.position, _startPosition) <= 0.1f)
        {
            Debug.Log("상태전환: Return -> Idle");
            _patrolTimer = 0f;
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // 행동: 시작 위치로 돌아간다.
        Vector3 direction = (_startPosition - transform.position).normalized;
        _characterController.Move(direction * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 전이 조건: 공격 범위 보다 멀어지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = AttackCoolTime;
            return;
        }

        // 행동 : 플레이어 공격
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= AttackCoolTime)
        {
            Debug.Log("플레이어 공격");
            _attackTimer = 0f;
        }
    }

    private IEnumerator Damaged_Coroutine()
    {
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DieTime);
        gameObject.SetActive(false);
    }

    private void Patrol()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("상태전환: Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }
        Vector3 direction = (_patrolPoints[_patrolIndex].position - transform.position).normalized;
        _characterController.Move(direction * MoveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, _patrolPoints[_patrolIndex].position) <= 0.1f)
        {
            _patrolIndex++;
           if(_patrolIndex >= _patrolPoints.Count)
            {
                _patrolIndex = 0;
            }
        }
    }
}
