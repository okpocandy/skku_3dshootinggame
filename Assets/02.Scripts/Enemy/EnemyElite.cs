using System.Collections;
using UnityEngine;

public class EnemyElite : EnemyFollow
{
    public float EliteAttackDistance = 5f;
    public float EliteMoveSpeed = 5f;
    public float ExplosionRadius = 5f;
    public int ExplosionDamage = 10;
    public float ExplosionKnockbackForce = 5f;
    
    public GameObject ExplosionEffectPrefab;
    public LayerMask ExplosionLayer;

    private bool _isExplode = false;

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

        MoveSpeed = EliteMoveSpeed;
        AttackDistance = EliteAttackDistance;
        ExplosionLayer = LayerMask.GetMask("Player");
    }

    private IEnumerator FindPlayerCoroutine()
    {
        while (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null)
            {
                base.Start();
                MoveSpeed = EliteMoveSpeed;
                AttackDistance = EliteAttackDistance;
                ExplosionLayer = LayerMask.GetMask("Player");
                break;
            }
            yield return null;
        }
    }

    protected override IEnumerator Die_Coroutine()
    {
        Explode();
        yield return null; // 폭발 후 즉시 파괴되므로 대기할 필요 없음
    }

    private void Explode()
    {
        _isExplode = true;
        if(ExplosionEffectPrefab != null)
        {
            Instantiate(ExplosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius, ExplosionLayer);

        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(new Damage { Value = ExplosionDamage, KnockbackForce = ExplosionKnockbackForce, From = this.gameObject});
            }
        }

        // 파괴, 죽음 효과
        EnemyPool.Instance.ReturnObject(this);
    }
}
