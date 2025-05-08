using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEliteHealth : UIEnemyHealth
{
    [Header("딜레이 체력바")]
    public GameObject DelaySliderPrefab;  // 딜레이 체력바 프리팹
    //public float DelayTime = 0.5f;        // 딜레이 시간
    public float SmoothSpeed = 5f;        // 부드러운 이동 속도

    private Slider _delaySlider;          // 딜레이 체력바
    private float _targetValue;           // 목표 체력값
    private Coroutine _delayCoroutine;    // 딜레이 코루틴

    protected override void Start()
    {
        WorldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas");
        TargetEnemy = this.GetComponent<Enemy>();
        
        
        // 딜레이 체력바 생성
        if (DelaySliderPrefab != null)
        {
            _delaySlider = Instantiate(DelaySliderPrefab, WorldSpaceCanvas.transform).GetComponent<Slider>();
            _delaySlider.value = 1f;
            _targetValue = 1f;
        }
        else
        {
            Debug.LogError("DelaySliderPrefab가 할당되지 않았습니다!");
        }

        // 이벤트 구독
        TargetEnemy.OnDamaged += UpdateHealth;
        TargetEnemy.OnDie += OnTargetDie;
    }

    protected override void Update()
    {  
        // 딜레이 체력바 위치 업데이트
        if (_delaySlider != null)
        {
            _delaySlider.transform.position = transform.position + Offset;
        }
    }

    public override void UpdateHealth()
    {
        _targetValue = TargetEnemy.Health / TargetEnemy.MaxHealth;
        
        if (_delayCoroutine != null)
        {
            StopCoroutine(_delayCoroutine);
        }
        
        _delayCoroutine = StartCoroutine(DelayHealthUpdate());
    }

    private IEnumerator DelayHealthUpdate()
    {
        //yield return new WaitForSeconds(DelayTime);

        while (Mathf.Abs(_delaySlider.value - _targetValue) > 0.001f)
        {
            _delaySlider.value = Mathf.Lerp(_delaySlider.value, _targetValue, Time.deltaTime * SmoothSpeed);
            yield return null;
        }

        _delaySlider.value = _targetValue;
    }

    protected override void OnTargetDie()
    {
        if (_delaySlider != null)
        {
            Destroy(_delaySlider.gameObject);
        }
    }
} 