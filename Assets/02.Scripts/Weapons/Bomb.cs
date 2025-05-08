using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject ExplosionEffectPrefab;
    public float ExplosionRadius = 5f;
    public int ExplosionDamage = 20;
    public float ExplosionKnockbackForce = 4f;
    public float DestroyTime = 10f;

    public LayerMask ExplosionLayer;

    private bool _isExploding = false;

    private void Start()
    {
        ExplosionLayer = LayerMask.GetMask("Enemy", "Damageable");
    }

    private void OnEnable()
    {
        _isExploding = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        gameObject.SetActive(false);
    }

    private void Explode()
    {
        if (_isExploding) return;  // 이미 폭발 중이면 중복 실행 방지
        _isExploding = true;
        
        // 카메라 쉐이크 효과 추가
        Camera.main.GetComponent<CameraFollow>()?.ShakeCamera(0.3f, 0.5f);

        if(ExplosionEffectPrefab != null)
        {
            Instantiate(ExplosionEffectPrefab, transform.position, Quaternion.identity);
        }
        
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius, ExplosionLayer);
        
        foreach(Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(new Damage { Value = ExplosionDamage, KnockbackForce = ExplosionKnockbackForce, From = this.gameObject});
            }
        }
    }
}
