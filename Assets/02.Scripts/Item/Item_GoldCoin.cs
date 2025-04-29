using UnityEngine;

public class Item_GoldCoin : MonoBehaviour
{
    public float RoataionSpeed = 10f;
    public float MoveSpeed = 5f;
    public float DetectRange = 10f;
    public float HeightOffset = 2f; // 곡선의 높이 조절
    public float GroundLevel = 0.5f; // 바닥 높이
    public float MidPointOffset = 0.5f;
    public float RandomHeightRange = 1f; // 높이 랜덤 범위
    public float RandomMidPointRange = 0.2f; // 중간점 위치 랜덤 범위

    private GameObject _player;
    private bool _isMoving = false;
    private float _moveProgress = 0f;
    private Vector3 _startPosition;
    private Vector3 _controlPoint;
    private float _randomHeight;
    private float _randomSpeed;
    private float _randomMidPoint;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // 각 동전마다 랜덤값 설정
        _randomHeight = Random.Range(-RandomHeightRange, RandomHeightRange);
        _randomMidPoint = Random.Range(-RandomMidPointRange, RandomMidPointRange);
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, RoataionSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if(distance <= DetectRange && !_isMoving)
        {
            StartMoving();
        }

        if (_isMoving)
        {
            MoveAlongBezierCurve();
        }
    }

    private void StartMoving()
    {
        _isMoving = true;
        _moveProgress = 0f;
        _startPosition = transform.position;
        
        // 제어점 설정 (시작점과 목표점 사이의 중간점)
        Vector3 midPoint = (_startPosition + _player.transform.position) * (MidPointOffset + _randomMidPoint);
        // 높이를 GroundLevel보다 높게 설정하고 랜덤값 추가
        _controlPoint = new Vector3(midPoint.x, 
            Mathf.Max(midPoint.y + HeightOffset + _randomHeight, GroundLevel + HeightOffset), 
            midPoint.z);
    }

    private void MoveAlongBezierCurve()
    {
        _moveProgress += Time.deltaTime * MoveSpeed;
        
        if (_moveProgress >= 1f)
        {
            _isMoving = false;
            // 플레이어에게 골드 추가하는 로직 추가
            Destroy(gameObject);
            return;
        }

        // 2차 베지에 곡선 계산
        Vector3 position = CalculateBezierPoint(_moveProgress, _startPosition, _controlPoint, _player.transform.position);
        
        // 바닥 아래로 내려가지 않도록 제한
        position.y = Mathf.Max(position.y, GroundLevel);
        
        transform.position = position;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2 * u * t * p1; // 2(1-t)t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }
}
