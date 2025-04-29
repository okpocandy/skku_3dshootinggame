using UnityEngine;

public class Melee : MonoBehaviour
{
    public float AttackRange = 5f;
    public float AttackAngle = 100f;
    public int AttackDamage = 20;
    public float AttackKnockback = 5f;
    public float AttackCoolTime = 1f;
    private float _attackCoolTimer = 0f;
    public Animator _animator;

    [SerializeField]
    private LayerMask _layerMask;
    private Collider[] hits = new Collider[32];
    private Transform _playerTransform;

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Enemy", "Damageable");
        _attackCoolTimer = AttackCoolTime;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        _attackCoolTimer += Time.deltaTime;

        if(_attackCoolTimer < AttackCoolTime)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            _attackCoolTimer = 0f;
            _animator.SetTrigger("Melee");
            //MeleeAttack();
        }
    }

    public void MeleeAttack()
    {
        var size = Physics.OverlapSphereNonAlloc(_playerTransform.position, AttackRange, hits, _layerMask);

        for (int i = 0; i < size; i++)
        {
            if (hits[i].TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Vector3 direction = (hits[i].transform.position - _playerTransform.position);
                direction.y = _playerTransform.forward.y;

                // 부채꼴 범위 안에 들어온다면 
                if (Vector3.Angle(direction, _playerTransform.forward) <= AttackAngle * .5f)
                {
                    damageable.TakeDamage(new Damage { Value = AttackDamage, KnockbackForce = AttackKnockback, From = transform.root.gameObject });
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_playerTransform == null)
            _playerTransform = transform.root;

        // 범위 그리기
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_playerTransform.position, AttackRange);

        // 시야각 선 그리기
        Gizmos.color = Color.red;
        float halfAngle = AttackAngle * 0.5f; // 시야각의 절반
        
        // 왼쪽 시야각 선
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * _playerTransform.forward;
        Gizmos.DrawRay(_playerTransform.position, leftDir * AttackRange);
        
        // 오른쪽 시야각 선
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * _playerTransform.forward;
        Gizmos.DrawRay(_playerTransform.position, rightDir * AttackRange);
    }
}
