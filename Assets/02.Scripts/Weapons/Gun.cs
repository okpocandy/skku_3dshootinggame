using System.Collections;
using UnityEngine;
using System;

public class Gun : MonoBehaviour
{
    [Header("총")]
    public ParticleSystem BulletEffect;
    public int MaxBulletCount = 50;
    public float KnockbackForce = 1f;
    public int Damage = 10;
    [SerializeField]
    private int _currentBulletCount = 0;
    public float FireRate = 0.1f;
    private float _fireTimer = 0f;
    
    [Header("총 재장전")]
    public float ReloadTime = 2f;
    private float _reloadTimer = 0f;
    [SerializeField]
    private bool _isReloading = false;
    private Camera _mainCamera;
    public GameObject FirePosition;

    private LineRenderer _bulletLineRenderer;
    private Vector3[] _linePositions = new Vector3[2];
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _trailTime;

    [SerializeField]
    private TrailRenderer _bulletTrailRenderer;
    [SerializeField]
    private float _maxDistance = 10f;

    public Action OnFire;

    private CameraFollow _cameraFollow;
    public Animator _animator;

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();
        _bulletLineRenderer.positionCount = 2;
        _bulletLineRenderer.enabled = false;
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _currentBulletCount = MaxBulletCount;
        _cameraFollow = FindAnyObjectByType<CameraFollow>();
    }

    void Update()
    {
        GunFire();
        ReloadGun();
    }

    private void GunFire()
    {
        if (_isReloading) return;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= FireRate)
        {
            if (Input.GetMouseButton(0) && _currentBulletCount > 0)
            {
                _animator.SetTrigger("Fire");
                
                _fireTimer = 0f;
                _currentBulletCount--;
                UIWeapons.Instance.UpdateBulletCount(_currentBulletCount, MaxBulletCount);

                Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _maxDistance))
                {
                    if (hit.collider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(new Damage { Value = Damage, KnockbackForce = KnockbackForce, From = transform.root.gameObject });
                    }

                    if(BulletEffect != null)
                    {
                        BulletEffect.transform.position = hit.point;
                        BulletEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                        BulletEffect.Play();
                    }
                    
                    StartCoroutine(TraceBullet(hit.point));
                    StartCoroutine(MoveTrail(hit.point));
                }
                else
                {
                    Vector3 targetPoint = ray.GetPoint(_maxDistance);
                    StartCoroutine(TraceBullet(targetPoint));
                    StartCoroutine(MoveTrail(targetPoint));
                }

                OnFire?.Invoke();
            }
        }
    }

    private IEnumerator MoveTrail(Vector3 position)
    {
        TrailRenderer trailRenderer = TrailPool.Instance.GetObject();
        if (trailRenderer == null) yield break;

        trailRenderer.transform.position = FirePosition.transform.position;
        _startPosition = trailRenderer.transform.position;
        _endPosition = position;
        _trailTime = 0f;

        while (_trailTime < 1f)
        {
            _trailTime += Time.deltaTime / trailRenderer.time;
            trailRenderer.transform.position = Vector3.Lerp(_startPosition, _endPosition, _trailTime);
            yield return null;
        }

        TrailPool.Instance.ReturnObject(trailRenderer);
    }

    private IEnumerator TraceBullet(Vector3 position)
    {
        _linePositions[0] = FirePosition.transform.position;
        _linePositions[1] = position;
        _bulletLineRenderer.SetPositions(_linePositions);
        _bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.05f);

        _bulletLineRenderer.enabled = false;
    }

    private void ReloadGun()
    {
        if (_isReloading)
        {
            _reloadTimer += Time.deltaTime;
            UIWeapons.Instance.UpdateReloadingSlider(_reloadTimer, ReloadTime);
            
            if (Input.GetMouseButton(0))
            {
                Debug.Log("재장전 실패");
                UIWeapons.Instance.DeactivateReloading();
                UIWeapons.Instance.DeactivateReloadingSlider();
                _isReloading = false;
                return;
            }
            
            if (_reloadTimer >= ReloadTime)
            {
                _currentBulletCount = MaxBulletCount;
                _isReloading = false;
                UIWeapons.Instance.UpdateBulletCount(_currentBulletCount, MaxBulletCount);
                UIWeapons.Instance.DeactivateReloading();
                UIWeapons.Instance.DeactivateReloadingSlider();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R) || _currentBulletCount <= 0)
            {
                _isReloading = true;
                _reloadTimer = 0f;
                UIWeapons.Instance.ActivateReloading();
                UIWeapons.Instance.ActivateReloadingSlider();
            }
        }
    }
}
