using System.Collections;
using UnityEngine;
using System;

public class Gun : MonoBehaviour
{
    [Header("총")]
    public ParticleSystem BulletEffect;
    public int MaxBulletCount = 50;
    public float KnockbackForce = 1f;
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

    [SerializeField]
    private TrailRenderer _bulletTrailRenderer;
    [SerializeField]
    private float _maxDistance = 10f;
    private TrailRenderer _bulletTrailRenderer2;

    public Action OnFire;

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();
        // 사용할 점을 두개로 변경
        _bulletLineRenderer.positionCount = 2;
        _bulletLineRenderer.enabled = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera = Camera.main;
        _currentBulletCount = MaxBulletCount;
    }

    // Update is called once per frame
    void Update()
    {
        GunFire();
        ReloadGun();
    }

    private void GunFire()
    {
        _fireTimer += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if(_fireTimer < FireRate || _currentBulletCount <= 0)
                return;

            _fireTimer = 0f;
            _currentBulletCount--;
            UIManager.Instance.UpdateBulletCount(_currentBulletCount, MaxBulletCount);
            CameraShake.Instance.ShakeCamera(Camera.main.transform.position);
            OnFire?.Invoke();

            // 레이저 생성
            Ray ray = new Ray(FirePosition.transform.position, _mainCamera.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);
            // 피격
            
            if(isHit)
            {
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal;
                BulletEffect.Play();
                TrailRenderer trailRenderer = TrailPool.Instance.GetTrail();
                if (trailRenderer != null)
                {
                    trailRenderer.transform.position = FirePosition.transform.position;
                    StartCoroutine(SpawnTrail(trailRenderer, hitInfo.point));
                }
                // 적이 맞았을 때때
                if(hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                    if(enemy != null)
                    {
                        enemy.TakeDamage(new Damage{Value = 10, From = this.gameObject});
                    }
                }
            }
            else
            {
                TrailRenderer trailRenderer = TrailPool.Instance.GetTrail();
                if (trailRenderer != null)
                {
                    trailRenderer.transform.position = FirePosition.transform.position;
                    StartCoroutine(SpawnTrail(trailRenderer, FirePosition.transform.position + _mainCamera.transform.forward * _maxDistance));
                }
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trailRenderer, Vector3 position)
    {
        float time = 0;
        Vector3 startPosition = trailRenderer.transform.position;

        while(time < 1)
        {
            trailRenderer.transform.position = Vector3.Lerp(startPosition, position, time);
            time += Time.deltaTime / trailRenderer.time;

            yield return null;
        }

        // Trail이 목적지에 도달하면 풀로 반환
        TrailPool.Instance.ReturnTrail(trailRenderer);
    }

    private IEnumerator TraceBullet(Vector3 position)
    {
        _bulletLineRenderer.SetPosition(0, FirePosition.transform.position);
        _bulletLineRenderer.SetPosition(1, position);
        _bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.05f);

        _bulletLineRenderer.enabled = false;
    }

    private void ReloadGun()
    {
        if(_isReloading)
        {
            _reloadTimer += Time.deltaTime;
            UIManager.Instance.UpdateReloadingSlider(_reloadTimer, ReloadTime);
            if(Input.GetMouseButton(0))
            {
                Debug.Log("재장전 실패");
                UIManager.Instance.DeactivateReloading();
                UIManager.Instance.DeactivateReloadingSlider();
                _isReloading = false;
                return;
            }
            if(_reloadTimer >= ReloadTime)
            {
                _currentBulletCount = MaxBulletCount;
                _isReloading = false;
                UIManager.Instance.UpdateBulletCount(_currentBulletCount, MaxBulletCount);
                UIManager.Instance.DeactivateReloading();
                UIManager.Instance.DeactivateReloadingSlider();
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R) || _currentBulletCount <= 0)
            {
                _isReloading = true;
                _reloadTimer = 0f;
                UIManager.Instance.ActivateReloading();
                UIManager.Instance.ActivateReloadingSlider();
            }
        }
    }
}
