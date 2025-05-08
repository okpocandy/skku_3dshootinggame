using UnityEngine;
using UnityEngine.AI;
public class EnemyEliteSpawner : MonoBehaviour
{
    public float SpawnTime = 30f;
    public float SpawnRadius = 15f;
    private float _spawnTimer = 0f;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if(_spawnTimer <= SpawnTime)
        {
            return;
        }
        _spawnTimer = 0;

        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * SpawnRadius;
        spawnPosition.y = 1.2f;

        EnemyElite enemy = EnemyElitePool.Instance.GetObject();
        enemy._startPosition = spawnPosition;
        
        // NavMeshAgent 초기화
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPosition);  // NavMeshAgent의 위치를 즉시 변경
            agent.enabled = true;       // NavMeshAgent 활성화
            agent.isStopped = false;    // 이동 시작
        }
        
        enemy.transform.position = spawnPosition;
    }
}
