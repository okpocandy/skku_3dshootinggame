using UnityEngine;

public class Melee : MonoBehaviour
{

    public float AttackRange = 3f;
    public float AttackAngle = 10f;
    public int AttackDamage = 20;
    public float AttackKnockback = 5f;


    [SerializeField]
    private LayerMask _layerMask;
    private Collider[] hits = new Collider[32];
    private void Start()
    {
        _layerMask = LayerMask.GetMask("Enemy", "Damageable");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, hits, _layerMask);

            for(int i=0; i<size; i++)
            {
                if(hits[i].TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    Vector3 direction = (hits[i].transform.position - transform.position);
                    direction.y = transform.forward.y;
                    
                    // 부채꼴 범위 안에 들어온다면 
                    if (Vector3.Angle(direction, transform.forward) <= AttackAngle * .5f)
                    {
                        damageable.TakeDamage(new Damage { Value = AttackDamage, KnockbackForce = AttackKnockback, From = this.gameObject});
                    }
                }
            }
        }
        
    }

    void OnDrawGizmos()
    {
        // 범위 그리기
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        // 시야각 선 그리기
        Gizmos.color = Color.red;
        float halfAngle = AttackAngle * 0.5f; // 시야각의 절반
        
        // 왼쪽 시야각 선
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDir * AttackRange);
        
        // 오른쪽 시야각 선
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rightDir * AttackRange);
    }
}
