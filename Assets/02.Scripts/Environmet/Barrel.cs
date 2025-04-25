using TMPro;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int Health = 10;

    public float ExplosionRadius = 10f;
    public int ExplosionDamage = 10;
    public float ExplosionKnockbackForce = 4f;
    public float ExplosionForce = 20f;
    public float DestroyTime = 10f;
    public LayerMask ExplosionLayer;

    private Rigidbody _rigidbody;
    private Collider[] _colliders = new Collider[32]; // Pre-allocate array
    private bool _isExploding = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ExplosionLayer = LayerMask.GetMask("Enemy", "Player", "Damageable");
        _rigidbody.isKinematic = true;
    }

    public void TakeDamage(Damage damage)
    {
        if(_isExploding) return;
        Health -= damage.Value;
        if (Health <= 0)
        {
            Fly();
            Explode();
        }
    }

    private void Fly()
    {
        Vector3 direction = new Vector3();
        direction.x = Random.Range(-1f, 1f);
        direction.y = 0.5f;
        direction.z = Random.Range(-1f, 1f);
        direction.Normalize();
        _rigidbody.isKinematic = false;

        _rigidbody.AddForce(direction * ExplosionForce, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.forward * 1f, ForceMode.Impulse);
    }

    private void Explode()
    {
        _isExploding = true;
        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);
        
        foreach(Collider hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(new Damage { Value = ExplosionDamage, KnockbackForce = ExplosionKnockbackForce, From = this.gameObject});
            }
        }
        
        Destroy(gameObject, DestroyTime);
    }

    void OnDestroy()
    {
        if (MapBake.Instance != null)
        {
            MapBake.Instance.Bake();
        }
    }
}
